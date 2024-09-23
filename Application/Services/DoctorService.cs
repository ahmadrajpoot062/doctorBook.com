using Core.Entities;
using Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<Doctor> getByEmail(string? email)
        {
            return await _doctorRepository.getByEmail(email);
        }

        public async Task<List<Doctor>> getBySpeciality(string speciality)
        {
            return await _doctorRepository.getBySpeciality(speciality);
        }
    }
}
