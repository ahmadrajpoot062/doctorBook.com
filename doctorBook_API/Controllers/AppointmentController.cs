using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace doctorBook_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IRepository<Appointment> _repository;

        public AppointmentController(IAppointmentRepository appointmentRepository, IRepository<Appointment> repository)
        {
            _appointmentRepository = appointmentRepository;
            _repository = repository;
        }

        
        [HttpGet("getAllAppointments")]
        public async Task<ActionResult<List<Appointment>>> GetAllAppointments()
        {
            var appointments = await _repository.GetAll();
            return Ok(appointments);
        }

        
        [HttpGet("GetAppointmentById/{id}")]
        public async Task<ActionResult<Appointment>> GetAppointmentById(int id)
        {
            var appointment = await _repository.GetById(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return Ok(appointment);
        }

        
        [HttpGet("GetAppointmentsByDoctorEmail/{docEmail}")]
        public async Task<ActionResult<List<Appointment>>> GetAppointmentsByDoctorEmail(string docEmail)
        {
            var appointments = await _appointmentRepository.getbyDoctorEmail(docEmail);
            if (appointments == null || appointments.Count == 0)
            {
                return NotFound("No appointments found for this doctor.");
            }
            return Ok(appointments);
        }

        
        [HttpGet("GetAppointmentsByPatientEmail/{patientEmail}")]
        public async Task<ActionResult<List<Appointment>>> GetAppointmentsByPatientEmail(string patientEmail)
        {
            var appointments = await _appointmentRepository.getbyPatientEmail(patientEmail);
            if (appointments == null || appointments.Count == 0)
            {
                return NotFound("No appointments found for this patient.");
            }
            return Ok(appointments);
        }

        
        [HttpGet("GetBookedSlots/{docEmail}/{date}")]
        public async Task<ActionResult<List<string>>> GetBookedSlots(string docEmail, DateTime date)
        {
            var bookedSlots = await _appointmentRepository.getBookedSlots(docEmail, date);
            if (bookedSlots == null || bookedSlots.Count == 0)
            {
                return NotFound("No booked slots found for this date.");
            }
            return Ok(bookedSlots);
        }

        
        [HttpPost("AddAppointment")]
        public async Task<ActionResult> AddAppointment([FromBody] Appointment appointment)
        {
            if (appointment == null)
            {
                return BadRequest("Appointment cannot be null.");
            }

            await _repository.Add(appointment);
            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.Id }, appointment);
        }

        
        [HttpPut("UpdateAppointment/{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateAppointment(int id, [FromBody] Appointment appointment)
        {
            if (appointment == null || id != appointment.Id)
            {
                return BadRequest("Appointment data is invalid.");
            }

            var existingAppointment = await _repository.GetById(id);
            if (existingAppointment == null)
            {
                return NotFound("Appointment not found.");
            }

            await _repository.Update(appointment);
            return NoContent();
        }

        
        [HttpDelete("DeleteAppointment/{id}")]
        public async Task<ActionResult> DeleteAppointment(int id)
        {
            var appointment = await _repository.GetById(id);
            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            await _repository.Delete(id);
            return NoContent();
        }
    }
}
