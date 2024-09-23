using System.ComponentModel.DataAnnotations;

namespace doctorBook.com.Models
{
    public class NoScriptTagsAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && value is string strValue && (strValue.Contains("<") || strValue.Contains(">")))
            {
                return new ValidationResult("The field cannot contain scripting tags like < or >");
            }
            return ValidationResult.Success;
        }
    }
}
