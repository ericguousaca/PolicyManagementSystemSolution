using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PolicyApiLibrary.Validations
{
    public class ValidStartDateString : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string dateStr = (string)value;

            DateTime startDate = DateTime.MinValue;
            if (!DateTime.TryParseExact(dateStr, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out startDate))
            {
                return new ValidationResult($"The Date String '{dateStr}' cannot be converted to be an valid date.");
            }
            else
            {
                if (startDate < DateTime.Today)
                {
                    return new ValidationResult($"The Start Date '{dateStr}' shall be on or later than current date {DateTime.Today.ToShortDateString()}.");
                }

                return ValidationResult.Success;
            }
        }
    }
}
