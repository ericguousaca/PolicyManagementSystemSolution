using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerApi.Validations
{
    public class ValidDateString : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string dateStr = (string)value;

            DateTime birthday = DateTime.MinValue;
            if (!DateTime.TryParse(dateStr, out birthday))
            {
                return new ValidationResult($"The Date String '{dateStr}' cannot be converted to be a valid date.");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
