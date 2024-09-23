using doctorBook.com.Data;
using System.ComponentModel.DataAnnotations;
using Application.Services;

namespace doctorBook.com.Models
{
	public class EmailExistsAttribute : ValidationAttribute
	{
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !(value is string email))
            {
                return ValidationResult.Success; // No need to validate null or non-string values
            }

            // Retrieve the GenericServices<Doctor> from the validation context
            var genericService = (GenericServices<Doctor>)validationContext.GetService(typeof(GenericServices<Doctor>));

            // Use the synchronous wrapper method to perform asynchronous validation
            var validationResult = ValidateEmailAsync(email, genericService).GetAwaiter().GetResult();

            return validationResult;
        }

        private async Task<ValidationResult> ValidateEmailAsync(string email, GenericServices<Doctor> genericService)
        {
            // Check if the email exists in the database asynchronously
            List<Doctor> doctors = await genericService.GetAll();
            foreach (var doctor in doctors)
            {
                if (doctor.Email == email)
                {
                    return new ValidationResult(ErrorMessage ?? "Doctor with this email address already exists.");
                }
            }

            return ValidationResult.Success;
        }

    }
}
