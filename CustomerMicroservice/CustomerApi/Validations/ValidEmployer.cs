using CustomerApi.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace CustomerApi.Validations
{
    public class ValidEmployer : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            CustomerViewModel vmCustomer = (CustomerViewModel)validationContext.ObjectInstance;
            string employer = (string)value;

            if (vmCustomer.EmployeeType == "salaried")
            {
                if (string.IsNullOrEmpty(employer.Trim()))
                {
                    return new ValidationResult($"The employer cannot be empty if the employee type is salaried.");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
