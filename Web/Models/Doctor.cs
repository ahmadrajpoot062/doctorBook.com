using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace doctorBook.com.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [EmailExists]
        public string Email { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters")]
        [NoSpaces(ErrorMessage = "First Name cannot contain spaces")]
        [NotAllowedWords("abc", "xyz", ErrorMessage = "First Name cannot contain words like abc/xyz")]
        public string First_Name { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters")]
        [NoSpaces(ErrorMessage = "Last Name cannot contain spaces")]
        [NotAllowedWords("abc", "xyz", ErrorMessage = "Last Name cannot contain words like abc/xyz")]
        public string Last_Name { get; set; }

        public string Speciality { get; set; }

        [Required(ErrorMessage = "Qualification is required")]
        [StringLength(150, ErrorMessage = "Qualification cannot be longer than 150 characters")]
        [NoScriptTags(ErrorMessage = "Qualification cannot contain scripting tags like < or >")]
        public string Qualification { get; set; }

        public string? Timings { get; set; }

        [Required(ErrorMessage = "Charges are required")]
        [Range(0, 10000, ErrorMessage = "Charges must be between 0 and 10000")]
        public decimal Charges { get; set; }

        [Required(ErrorMessage = "Experience is required")]
        [Range(0, 100, ErrorMessage = "Experience must be between 0 and 100 years")]
        public int Experiance { get; set; }

        [Required(ErrorMessage = "Consultation Time is required")]
        [Range(1, 120, ErrorMessage = "Consultation Time must be between 1 and 120 minutes")]
        public int Consultation_Time { get; set; }

        [Required(ErrorMessage = "Wait Time is required")]
        [Range(0, 120, ErrorMessage = "Wait Time must be between 0 and 120 minutes")]
        public int Wait_Time { get; set; }

        [Required(ErrorMessage = "Diagnosis Satisfaction is required")]
        [Range(1, 100, ErrorMessage = "Diagnosis Satisfaction must be between 1 and 100%")]
        public int Diagnosis_Satisfaction { get; set; }

        public IFormFile? Image { get; set; }
        public string? ImagePath { get; set; }
    }
}
