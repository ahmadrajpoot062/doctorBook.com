using System.ComponentModel.DataAnnotations;

namespace doctorBook.com.Models
{
    public class NoSpacesAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && value is string strValue && strValue.Contains(" "))
            {
                return new ValidationResult("The field cannot contain spaces");
            }
            return ValidationResult.Success;
        }
    }
}
