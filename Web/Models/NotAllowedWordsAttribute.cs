using System.ComponentModel.DataAnnotations;

namespace doctorBook.com.Models
{
    public class NotAllowedWordsAttribute : ValidationAttribute
    {
        private readonly string[] _disallowedWords;

        public NotAllowedWordsAttribute(params string[] disallowedWords)
        {
            _disallowedWords = disallowedWords;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && value is string strValue)
            {
                foreach (var word in _disallowedWords)
                {
                    if (strValue.ToLower() == word.ToLower())
                    {
                        return new ValidationResult($"The field cannot contain the word '{word}'");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}

