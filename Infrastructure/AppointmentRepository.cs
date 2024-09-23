using Core.Entities;
using Core.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private const string CONNECTION_STRING = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=doctorBookDB;Integrated Security=True;";

        public async Task<List<string>> getBookedSlots(string docEmail, DateTime date)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            {
                try
                {
                    await conn.OpenAsync();
                    string select_query = "select AppointmentTime from Appointment where DoctorEmail=@e and AppointmentDate=@d and AppointmentStatus='Accepted'";
                    return (await conn.QueryAsync<string>(select_query, new { e = docEmail, d = date })).ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error! " + ex);
                    throw;
                }
            }
        }

        public async Task<List<Appointment>> getbyDoctorEmail(string docEmail)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            {
                try
                {
                    await conn.OpenAsync();
                    string select_query = "select * from Appointment where DoctorEmail=@d";
                    return (await conn.QueryAsync<Appointment>(select_query, new { d = docEmail })).ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error! " + ex);
                    throw;
                }
            }
        }

        public async Task<List<Appointment>> getbyPatientEmail(string patientEmail)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            {
                try
                {
                    await conn.OpenAsync();
                    string select_query = "select * from Appointment where UserEmail=@d";
                    return (await conn.QueryAsync<Appointment>(select_query, new { d = patientEmail })).ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error! " + ex);
                    throw;
                }
            }
        }
    }
}
