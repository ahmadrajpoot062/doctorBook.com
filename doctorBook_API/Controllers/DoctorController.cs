using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace doctorBook_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IRepository<Doctor> _repository;

        public DoctorController(IDoctorRepository doctorRepository, IRepository<Doctor> repository)
        {
            _doctorRepository = doctorRepository;
            _repository = repository;
        }

        
        [HttpGet("getAllDoctors")]
        public async Task<ActionResult<List<Doctor>>> GetAllDoctors()
        {
            var doctors = await _repository.GetAll();
            return Ok(doctors);
        }


        [HttpGet("GetDoctorById/{id}")]
        public async Task<ActionResult<Doctor>> GetDoctorById(int id)
        {
            var doctor = await _repository.GetById(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return Ok(doctor);
        }


        [HttpGet("GetDoctorsBySpeciality/{speciality}")]
        public async Task<ActionResult<List<Doctor>>> GetDoctorsBySpeciality(string speciality)
        {
            var doctors = await _doctorRepository.getBySpeciality(speciality);
            if (doctors == null || doctors.Count == 0)
            {
                return NotFound();
            }
            return Ok(doctors);
        }


        [HttpGet("GetDoctorByEmail/{email}")]
        public async Task<ActionResult<Doctor>> GetDoctorByEmail(string email)
        {
            var doctor = await _doctorRepository.getByEmail(email);
            if (doctor == null)
            {
                return NotFound();
            }
            return Ok(doctor);
        }


        [HttpPost("AddDoctor")]
        public async Task<ActionResult> AddDoctor([FromBody] Doctor doctor)
        {
            if (doctor == null)
            {
                return BadRequest("Doctor cannot be null.");
            }

            await _repository.Add(doctor);
            return CreatedAtAction(nameof(GetDoctorById), new { id = doctor.Id }, doctor);
        }


        [HttpPut("UpdateDoctor/{id}")]
        public async Task<ActionResult> UpdateDoctor(int id, [FromBody] Doctor doctor)
        {
            if (doctor == null || id != doctor.Id)
            {
                return BadRequest("Doctor data is invalid.");
            }

            var existingDoctor = await _repository.GetById(id);
            if (existingDoctor == null)
            {
                return NotFound("Doctor not found.");
            }

            await _repository.Update(doctor);
            return NoContent();
        }


        [HttpDelete("DeleteDoctor/{id}")]
        public async Task<ActionResult> DeleteDoctor(int id)
        {
            var doctor = await _repository.GetById(id);
            if (doctor == null)
            {
                return NotFound("Doctor not found.");
            }

            await _repository.Delete(id);
            return NoContent();
        }
    }
}
