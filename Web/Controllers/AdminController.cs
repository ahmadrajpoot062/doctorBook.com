using Application.Services;
using doctorBook.com.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.IO;
using System.Runtime.CompilerServices;
using static System.Net.WebRequestMethods;

namespace doctorBook.com.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly GenericServices<Doctor> _doc;
        private readonly GenericServices<Core.Entities.Appointment> _app;
        private readonly GenericServices<Core.Entities.AspNetUsers> _users;
        private readonly IMemoryCache _memoryCache;
        public AdminController(ILogger<AdminController> logger, IWebHostEnvironment env, GenericServices<Doctor> doc, GenericServices<Core.Entities.Appointment> app, GenericServices<Core.Entities.AspNetUsers> users,IMemoryCache memoryCache)
        {
            _logger = logger;
            _env = env;
            _doc = doc;
            _app = app;
            _users = users;
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Search(string category)
        {
            if (category == null)
            {
                return PartialView("_viewDoctorsPartial", await _doc.GetAll());
            }
            else
            {
                List<Doctor> list = await _doc.GetAll();
                var filteredList = list.Where(a => a.Speciality.Contains(category, StringComparison.OrdinalIgnoreCase)).ToList();
                return PartialView("_viewDoctorsPartial", filteredList);
            }
        }

        [HttpGet]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Client)]
        public IActionResult Add_Doctor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add_Doctor(Doctor d)
        {
            // Check if model state is valid
            if (!ModelState.IsValid)
            {
                return View(d);
            }

            // Construct the timings string
            d.Timings = Request.Form["StartHour"] + ":00" + Request.Form["StartAMPM"] + "-" + Request.Form["EndHour"] + ":00" + Request.Form["EndAMPM"];
            d.ImagePath = @"\UploadedFiles\DefaultImage\blankUserImage.png";

            // Add doctor to the database
            await _doc.Add(d);

            // Invalidate the cache
            _memoryCache.Remove("doctorsList");

            // Redirect to the View_Doctors action
            return RedirectToAction("View_Doctors", "Admin");
        }

        public async Task<IActionResult> Delete_Doctor()
        {
            return View(await _doc.GetAll());
        }

        [HttpPost]
        public async Task<ActionResult> Delete_Doctor(int id)
        {
            await _doc.Delete(id);

            // Invalidate the cache
            _memoryCache.Remove("doctorsList");

            return PartialView("_deleteDoctorsPartial", await _doc.GetAll());
        }
        public async Task<IActionResult> Update_Doctor()
        {
            //IRepository<Doctor> iRepository = new GenericRepository<Doctor>(CONNECTION_STRING);
            return View(await _doc.GetAll());
        }
        public async Task<IActionResult> Update_Doctor_Form()
        {
            string? id = Request.Query["id"];
            //IRepository<Doctor> iRepository = new GenericRepository<Doctor>(CONNECTION_STRING);
            return View(await _doc.GetById(int.Parse(id)));
        }

        [HttpPost]
        public async Task<ActionResult> Update_Doctor(Doctor d)
        {
            d.Timings = Request.Form["StartHour"] + ":00" + Request.Form["StartAMPM"] + "-" + Request.Form["EndHour"] + ":00" + Request.Form["EndAMPM"];
            string? id = Request.Query["id"];
            d.Id = int.Parse(id);
            Doctor d1 = await _doc.GetById(d.Id);
            d.ImagePath = d1.ImagePath;

            await _doc.Update(d);

            // Invalidate the cache
            _memoryCache.Remove("doctorsList");
            _memoryCache.Remove($"doctorProfile_{d.Id}");

            return RedirectToAction("Update_Doctor", "Admin");
        }

        [ResponseCache(Duration = 60000, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> View_Doctors()
        {
            string cacheKey = "doctorsList";
            if (!_memoryCache.TryGetValue(cacheKey, out List<Doctor> doctors))
            {
                doctors = await _doc.GetAll();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10)); // Cache for 10 minutes

                _memoryCache.Set(cacheKey, doctors, cacheEntryOptions);
            }

            return View(doctors);
        }

        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> Doctor_Profile()
        {
            string id = Request.Query["id"];
            string cacheKey = $"doctorProfile_{id}";
            if (!_memoryCache.TryGetValue(cacheKey, out Doctor doctor))
            {
                doctor = await _doc.GetById(int.Parse(id));
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10)); // Cache for 10 minutes

                _memoryCache.Set(cacheKey, doctor, cacheEntryOptions);
            }

            return View(doctor);
        }

        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> View_Users()
        {
            return View(await _users.GetAll());
        }

        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> View_Appointments()
        {
            string cacheKey = "appointmentsList";
            if (!_memoryCache.TryGetValue(cacheKey, out List<Core.Entities.Appointment> appointments))
            {
                appointments = await _app.GetAll();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10)); // Cache for 10 minutes

                _memoryCache.Set(cacheKey, appointments, cacheEntryOptions);
            }

            return View(appointments);
        }

    }
}
