using CustomerApi.Models;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CustomerApi.Validations
{
    public class ValidEmployeeType : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get configuration service from context
            var configuration = (IConfiguration)validationContext.GetService(typeof(IConfiguration));

            EmployeeType[] employeeTypes = configuration.GetSection("EmployeeTypes").Get<EmployeeType[]>();

            string employeeType = (string)value;

            if (employeeTypes.Where(e => e.TypeName == employeeType).FirstOrDefault() == null)
            {
                string validEmpTypes = "";
                foreach (EmployeeType empTye in employeeTypes)
                {
                    validEmpTypes = validEmpTypes + "'" + empTye.TypeName + "' ";
                }
                validEmpTypes = validEmpTypes.Trim();

                return new ValidationResult($"The meployee type {employeeType} is incorrect. Valid Employee Types are: {validEmpTypes}");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
