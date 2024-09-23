using Application.Services;
using Core.Entities;
using doctorBook.com.Models;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace doctorBook.com.Controllers
{
    [Authorize(Policy ="DoctorPolicy")]
    public class DoctorController : Controller
    {
        private readonly ILogger<DoctorController> _logger;
        private readonly IWebHostEnvironment _env;
        private const string CONNECTION_STRING = "Server=(localdb)\\mssqllocaldb;Database=doctorBookDB;Trusted_Connection=True;MultipleActiveResultSets=true";
        private readonly AppointmentService _appointmentService;
        private readonly DoctorService _doctorService;
        private readonly GenericServices<doctorBook.com.Models.Doctor> _doc;
        private readonly GenericServices<Appointment> _app;
        private readonly IHubContext<Infrastructure.Hubs.AppointmentHub> _hubContext;
        public DoctorController(ILogger<DoctorController> logger, IWebHostEnvironment env, AppointmentService appointmentService, DoctorService doctorService, GenericServices<doctorBook.com.Models.Doctor> doc, GenericServices<Appointment> app, IHubContext<Infrastructure.Hubs.AppointmentHub> hubContext)
        {
            _logger = logger;
            _env = env;
			_appointmentService = appointmentService;
			_doctorService = doctorService;
            _doc = doc;
            _app= app;
            _hubContext = hubContext;
        }
        public async Task<IActionResult> Index()
        {
            //string data = String.Empty;
            //if (HttpContext.Request.Cookies.ContainsKey("first_request"))
            //{
            //    string firstVisitedDateTime = HttpContext.Request.Cookies["first_request"];
            //    data = "Welcome back " + firstVisitedDateTime;

            //}
            //else
            //{
            //    CookieOptions option = new CookieOptions();
            //    option.Expires = System.DateTime.Now.AddDays(1);
            //    data = "you visited first time";
            //    HttpContext.Response.Cookies.Append("first_request", System.DateTime.Now.ToString(), option);
            //}

            //ViewData["Data"]= data;
            //IRepository<Doctor> iRepository = new GenericRepository<Doctor>(CONNECTION_STRING);

            List<doctorBook.com.Models.Doctor> mvcDoctors = await _doc.GetAll();

            // Manual mapping from doctorBook.com.Models.Doctor to Core.Entities.Doctor
            List<Core.Entities.Doctor> coreDoctors = mvcDoctors.Select(mvcDoctor => new Core.Entities.Doctor
            {
                Id = mvcDoctor.Id,
                Email = mvcDoctor.Email,
                First_Name = mvcDoctor.First_Name,
                Last_Name = mvcDoctor.Last_Name,
                Speciality = mvcDoctor.Speciality,
                Qualification = mvcDoctor.Qualification,
                Timings = mvcDoctor.Timings,
                Charges = mvcDoctor.Charges,
                Experiance = mvcDoctor.Experiance,
                Consultation_Time = mvcDoctor.Consultation_Time,
                Wait_Time = mvcDoctor.Wait_Time,
                Diagnosis_Satisfaction = mvcDoctor.Diagnosis_Satisfaction,
                ImagePath = mvcDoctor.ImagePath,
            }).ToList();

            return View(coreDoctors);            
        }

        [HttpGet]
        public async Task<IActionResult> View_Profile()
        {
            string? userEmail = User.Identity?.Name;            
            return View(await _doctorService.getByEmail(userEmail));
        }

        [HttpPost]
        public async Task<IActionResult> View_Profile(IFormFile picture, int doctorId)
        {
            doctorBook.com.Models.Doctor d = await _doc.GetById(doctorId);
            string path= getImageUrl(picture);
            d.ImagePath = path;
			await _doc.Update(d);
            return PartialView("_doctorNewPicture",path);
        }

        public string getImageUrl(IFormFile myFile)
        {
            if (myFile != null && myFile.Length > 0)
            {
                string folderPath = Path.Combine("UploadedFiles", "DoctorImages"); // Relative path from wwwroot
                string fileName = Path.Combine(folderPath, Guid.NewGuid().ToString() + myFile.FileName);
                string wwwRootPath = _env.WebRootPath;
                if (!Directory.Exists(Path.Combine(wwwRootPath, folderPath))) // Check if path exists relative to wwwroot
                {
                    Directory.CreateDirectory(Path.Combine(wwwRootPath, folderPath)); // Create directory if needed
                }

                string filePath = Path.Combine(wwwRootPath, fileName); // Full path for saving
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    myFile.CopyTo(fileStream);
                }
                return $"\\{fileName}"; // Return file path relative to wwwroot
            }
            return string.Empty;
        }

        public async Task<IActionResult> Appointments()
        {         
            return View(await _appointmentService.getbyDoctorEmail(User.Identity?.Name));
        }
        public async Task<IActionResult> AcceptRequest()
        {
            string? id = Request.Query["id"];
            //IRepository<Appointment> iRepository = new GenericRepository<Appointment>(CONNECTION_STRING);
            Appointment appointment = await _app.GetById(int.Parse(id));
            appointment.AppointmentStatus = "Accepted";
            await _app.Update(appointment);

            await _hubContext.Clients.All.SendAsync("ReceiveSlotUpdate", appointment.DoctorEmail, appointment.AppointmentDate.ToString("yyyy-MM-dd"), appointment.AppointmentTime, true);

            return RedirectToAction("Appointments","Doctor");
        }
        public async Task<IActionResult> RejectRequest()
        {
            string? id = Request.Query["id"];
            //IRepository<Appointment> iRepository = new GenericRepository<Appointment>(CONNECTION_STRING);
            await _app.Delete(int.Parse(id));
            return RedirectToAction("Appointments", "Doctor");
        }
    }
}
