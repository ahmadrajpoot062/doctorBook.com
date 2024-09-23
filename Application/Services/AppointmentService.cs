using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<List<string>> getBookedSlots(string docEmail, DateTime date)
        {
            return await _appointmentRepository.getBookedSlots(docEmail, date);
        }

        public async Task<List<Appointment>> getbyDoctorEmail(string docEmail)
        {
            return await _appointmentRepository.getbyDoctorEmail(docEmail);
        }

        public async Task<List<Appointment>> getbyPatientEmail(string patientEmail)
        {
            return await _appointmentRepository.getbyPatientEmail(patientEmail);
        }
    }
}
