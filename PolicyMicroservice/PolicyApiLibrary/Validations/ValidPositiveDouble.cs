using System.ComponentModel.DataAnnotations;

namespace PolicyApiLibrary.Validations
{
    public class ValidPositiveDouble : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            double doubleValue = (double)value;

            if (doubleValue <= 0)
            {
                return new ValidationResult(null);
            }
            else
            {
                return ValidationResult.Success;
            }

        }
    }
}
