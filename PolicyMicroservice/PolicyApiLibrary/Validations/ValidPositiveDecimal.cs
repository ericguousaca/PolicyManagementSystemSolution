using System.ComponentModel.DataAnnotations;

namespace PolicyApiLibrary.Validations
{
    public class ValidPositiveDecimal : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            decimal decimalValue = (decimal)value;

            if (decimalValue <= 0)
            {
                // return new ValidationResult($"The input '{decimalValue}' must be positive value.");
                return new ValidationResult(null);
            }
            else
            {
                return ValidationResult.Success;
            }

        }
    }
}
