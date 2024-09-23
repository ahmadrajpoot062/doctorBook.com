using Application.Services;
using Core.Entities;
using doctorBook.com.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;

namespace doctorBook.com.Controllers
{
    public class AppointmentController : Controller
    {
        private const string CONNECTION_STRING = "Server=(localdb)\\mssqllocaldb;Database=doctorBookDB;Trusted_Connection=True;MultipleActiveResultSets=true";
        private readonly AppointmentService _appointmentSerive;
        private readonly DoctorService _doctorSerive;
		private readonly GenericServices<doctorBook.com.Models.Doctor> _doc;
		private readonly GenericServices<Core.Entities.Appointment> _app;

        public AppointmentController(AppointmentService appointmentService, DoctorService doctorSerive, GenericServices<doctorBook.com.Models.Doctor> doc, GenericServices<Core.Entities.Appointment> app)
        {
			_appointmentSerive = appointmentService;
			_doctorSerive = doctorSerive;
            _doc = doc;
            _app = app;
        }

        [Authorize(Policy = "PatientPolicy")]
        public async Task<IActionResult> Book_Appointment()
        {
            string? id = Request.Query["id"];
            //IRepository<Doctor> iRepository = new GenericRepository<Doctor>(CONNECTION_STRING);
            doctorBook.com.Models.Doctor doctor = await _doc.GetById(int.Parse(id));            
            return View(doctor);
        }

        [HttpPost]
		public async Task<IActionResult> Book_Appointment(DateTime date, string doctorEmail)
		{
			List<string> bookedSlots = await _appointmentSerive.getBookedSlots(doctorEmail, date);
			Core.Entities.Doctor coreDoctor = await _doctorSerive.getByEmail(doctorEmail);

            // Manual mapping from Core.Entities.Doctor to doctorBook.com.Models.Doctor
            doctorBook.com.Models.Doctor mvcDoctor = new doctorBook.com.Models.Doctor
            {
                Id = coreDoctor.Id,
				Email = coreDoctor.Email,
				First_Name = coreDoctor.First_Name,
				Last_Name = coreDoctor.Last_Name,
				Speciality = coreDoctor.Speciality,
				Qualification = coreDoctor.Qualification,
				Timings = coreDoctor.Timings,
				Charges = coreDoctor.Charges,
				Experiance = coreDoctor.Experiance,
				Consultation_Time = coreDoctor.Consultation_Time,
				Wait_Time = coreDoctor.Wait_Time,
				Diagnosis_Satisfaction = coreDoctor.Diagnosis_Satisfaction,
                ImagePath= coreDoctor.ImagePath,
			};

			List<string> availableSlots = BreakTimeRange(mvcDoctor.Timings, mvcDoctor.Consultation_Time);
			DoctorSlots doctorSlots = new DoctorSlots
			{
				Doctor = mvcDoctor,
				AvailableSlots = availableSlots,
				BookedSlots = bookedSlots,
				AppointmentDate = date
			};
			return PartialView("_availableSlots", doctorSlots);
		}

		public static List<string> BreakTimeRange(string rangeString, int interval = 30)
        {
            if (string.IsNullOrEmpty(rangeString))
            {
                throw new ArgumentNullException(nameof(rangeString));
            }

            string[] parts = rangeString.Split('-');
            if (parts.Length != 2)
            {
                throw new ArgumentException("Invalid time range format. Expected 'StartTime-EndTime'");
            }

            string startTimeString = parts[0].Trim();
            string endTimeString = parts[1].Trim();

            var startTime = ParseTime(startTimeString);
            var endTime = ParseTime(endTimeString);

            List<string> slots = new List<string>();
            var currentTime = startTime;

            // Adjust endTime if it's the next day
            if (startTime > endTime)
            {
                endTime = endTime.AddDays(1);
            }

            while (currentTime < endTime)
            {
                var nextTime = currentTime.AddMinutes(interval);
                if (nextTime > endTime)
                {
                    nextTime = endTime;
                }
                slots.Add($"{currentTime:hh:mm tt}-{nextTime:hh:mm tt}");
                currentTime = nextTime;
            }

            return slots;
        }

        private static DateTime ParseTime(string timeString)
        {
            if (DateTime.TryParseExact(timeString, "h:mmtt", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException("Invalid time format. Expected 'h:mmtt'");
            }
        }
        public async Task<IActionResult> SearchByDate(DateTime? date)
        {
			if (date == null)
			{
				List<Core.Entities.Appointment> allAppointments = await _appointmentSerive.getbyPatientEmail(User.Identity?.Name);
				return PartialView("_appointmentRequestsPartial", allAppointments);
			}
			List<Core.Entities.Appointment> appointments = await _appointmentSerive.getbyPatientEmail(User.Identity?.Name);
			List<Core.Entities.Appointment> filteredList = new List<Core.Entities.Appointment>();
			foreach (var appointment in appointments)
			{
				if (appointment.AppointmentDate.Equals(date))
				{
					filteredList.Add(appointment);
				}
			}
			return PartialView("_appointmentRequestsPartial", filteredList);

		}
		public async Task<IActionResult> Request_Submitted(string time, string email, DateTime appointmentDate , string firstName, string lastName)
        {
            Core.Entities.Appointment appointment = new Core.Entities.Appointment { UserEmail = User.Identity?.Name, DoctorEmail = email, DoctorName=firstName+" "+lastName , AppointmentDate = appointmentDate, AppointmentTime = time,AppointmentStatus="Pending" };
            //IRepository<Appointment> iRepository = new GenericRepository<Appointment>(CONNECTION_STRING);
            await _app.Add(appointment);           
            return View(await _doctorSerive.getByEmail(email));
        }

        [Authorize(Policy = "PatientPolicy")]
        public async Task<IActionResult> Cancel_Appointment(int appointmentId,string email)
        {            
            //IRepository<Appointment> iRepository = new GenericRepository<Appointment>(CONNECTION_STRING);
            await _app.Delete(appointmentId);
            return View(await _doctorSerive.getByEmail(email));          
        }
    }
}
