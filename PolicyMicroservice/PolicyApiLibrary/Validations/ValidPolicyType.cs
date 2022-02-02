using Microsoft.Extensions.Configuration;
using PolicyApiLibrary.DbModels;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PolicyApiLibrary.Validations
{
    public class ValidPolicyType : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get configuration service from context
            var configuration = (IConfiguration)validationContext.GetService(typeof(IConfiguration));

            PolicyType[] policyTypes = configuration.GetSection("PolicyTypes").Get<PolicyType[]>();

            string policyType = (string)value;

            if (policyTypes.Where(e => e.TypeName == policyType).FirstOrDefault() == null)
            {
                string validPolicyTypes = string.Join(", ", policyTypes.Select(x => x.TypeName).ToList());

                return new ValidationResult($"The policy type {policyType} is incorrect. Valid Policy Type must be one of '{validPolicyTypes}'");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
