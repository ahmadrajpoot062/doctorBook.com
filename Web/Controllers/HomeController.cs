using Application.Services;
using doctorBook.com.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace doctorBook.com.Controllers
{    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private const string CONNECTION_STRING = "Server=(localdb)\\mssqllocaldb;Database=doctorBookDB;Trusted_Connection=True;MultipleActiveResultSets=true";
        private readonly AppointmentService _appointmentService;
        private readonly DoctorService _doctorService;
        private readonly GenericServices<doctorBook.com.Models.Doctor> _doc;
        public HomeController(ILogger<HomeController> logger, AppointmentService appointmentService, GenericServices<doctorBook.com.Models.Doctor> doc, DoctorService doctorService)
        {
            _logger = logger;
			_appointmentService = appointmentService;
			_doctorService = doctorService;
            _doc = doc;
        }
        public async Task<IActionResult> Index()
        {
            if (User.HasClaim(claim => claim.Type == ClaimTypes.Email && claim.Value == "Admin@doctorBook.com"))
            {
                // User is an admin, redirect to AdminController's Index method
                return RedirectToAction("Index", "Admin");
            }
            else if (User.HasClaim(claim => claim.Type == "Role" && claim.Value == "Doctor"))
            {
                // User is a doctor, redirect to DoctorController's Index method
                return RedirectToAction("Index", "Doctor");
            }
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
			//ViewBag.Data = data;
			//IRepository<Doctor> iRepository = new GenericRepository<Doctor>(CONNECTION_STRING);


			// User is neither admin nor doctor, display the default Index view
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
                ImagePath=mvcDoctor.ImagePath,
			}).ToList();

			// Pass the mapped list to the view (if needed, adjust as per your use case)
			return View(coreDoctors);
		}

        public async Task<IActionResult> Doctors_Cardiologist()
        {           
            return View(await _doctorService.getBySpeciality("Cardiologist"));
        }

        public async Task<IActionResult> Doctors_Dentist()
        {
            return View(await _doctorService.getBySpeciality("Dentist"));
        }

        public async Task<IActionResult> Doctors_Neurologist()
        {
            return View(await _doctorService.getBySpeciality("Neurologist"));
        }

        public async Task<IActionResult> Doctors_EyeSpecialist()
        {
            return View(await _doctorService.getBySpeciality("Eye-Specialist"));
        }
        public IActionResult About()
        {
            return View();
        }

        [Authorize]
        public IActionResult Contact_Us()
        {
            return View();
        }
        public IActionResult FeedBack()
        {
            return View();
        }
        public IActionResult Terms_of_use()
        {
            return View();
        }
        public async Task<IActionResult> Doctor_Profile()
        {            
            string? id=Request.Query["id"];
            //IRepository<Doctor> iRepository = new GenericRepository<Doctor>(CONNECTION_STRING);
            return View(await _doc.GetById(int.Parse(id)));
        }

        [Authorize(Policy = "PatientPolicy")]
        public async Task<IActionResult> ViewAppointments()
        {
            return View(await _appointmentService.getbyPatientEmail(User.Identity?.Name));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
