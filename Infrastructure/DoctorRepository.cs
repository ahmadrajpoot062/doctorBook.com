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
    public class DoctorRepository : IDoctorRepository
    {
        private const string CONNECTION_STRING = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=doctorBookDB;Integrated Security=True;";

        public async Task<Doctor> getByEmail(string? email)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            {
                try
                {
                    await conn.OpenAsync();
                    string select_query = "select * from Doctor where Email=@Email";
                    return await conn.QuerySingleOrDefaultAsync<Doctor>(select_query, new { Email = email });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error! " + ex);
                    throw;
                }
            }
        }

        public async Task<List<Doctor>> getBySpeciality(string speciality)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            {
                speciality = speciality.ToLower();
                try
                {
                    await conn.OpenAsync();
                    string select_query = "select * from Doctor where lower(Speciality)=@s";
                    var doctors = await conn.QueryAsync<Doctor>(select_query, new { s = speciality });
                    return doctors.ToList();
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
