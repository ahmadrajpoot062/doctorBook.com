using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class Doctor
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string First_Name { get; set; }
		public string Last_Name { get; set; }
		public string Speciality { get; set; }
		public string Qualification { get; set; }
		public string? Timings { get; set; }
		public decimal Charges { get; set; }
		public int Experiance { get; set; }
		public int Consultation_Time { get; set; }
		public int Wait_Time { get; set; }
		public int Diagnosis_Satisfaction { get; set; }
		//public IFormFile? Image { get; set; }
		public string? ImagePath { get; set; }
	}
}
