using Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<List<string>> getBookedSlots(string docEmail, DateTime date);
        Task<List<Appointment>> getbyDoctorEmail(string docEmail);
        Task<List<Appointment>> getbyPatientEmail(string patientEmail);
    }
}
