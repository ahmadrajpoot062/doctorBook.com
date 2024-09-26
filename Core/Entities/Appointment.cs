using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class Appointment
	{
		public int  Id { get; set; }
		public string  UserEmail { get; set; }
		public string DoctorEmail { get; set; }
		public string DoctorName { get; set; }
		public DateTime AppointmentDate { get; set; }
		public string AppointmentTime { get; set; }
		public string AppointmentStatus { get; set; }
	}
}
