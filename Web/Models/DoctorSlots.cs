namespace doctorBook.com.Models
{
    public class DoctorSlots
    {
        public Doctor Doctor { get; set; }

        public List<string> AvailableSlots { get; set; }
        public List<string> BookedSlots { get; set; }
        public DateTime AppointmentDate { get; set; }

    }
}
