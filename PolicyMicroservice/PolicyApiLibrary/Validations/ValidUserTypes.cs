using Microsoft.Extensions.Configuration;
using PolicyApiLibrary.DbModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PolicyApiLibrary.Validations
{
    public class ValidUserTypes : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get configuration service from context
            var configuration = (IConfiguration)validationContext.GetService(typeof(IConfiguration));

            UserType[] userTypes = configuration.GetSection("UserTypes").Get<UserType[]>();

            List<string> inputUserTypes = (List<string>)value;

            //Check duplicate User Types
            HashSet<string> hsTypes = new HashSet<string> { };
            List<string> duplicateTypeList = new List<string> { };
            foreach (string type in inputUserTypes)
            {
                if (hsTypes.Contains(type))
                {
                    if (!duplicateTypeList.Contains(type))
                    {
                        duplicateTypeList.Add(type);
                    }
                }
                else
                {
                    hsTypes.Add(type);
                }
            }

            // Check invalid User Type
            List<string> invalidUserTypes = new List<string> { };

            foreach (string inputUserType in inputUserTypes)
            {
                if (userTypes.Where(x => x.TypeName == inputUserType.Trim().ToUpper()).FirstOrDefault() == null)
                {
                    invalidUserTypes.Add(inputUserType);
                }
            }

            string errMesg = "";

            if (invalidUserTypes.Count > 0)
            {
                errMesg = errMesg + $"The Input User Type(s) '{string.Join(",", invalidUserTypes)}' invalid. Valid User Type(s) is one or mixed of any type in '{string.Join(", ", userTypes.Select(x => x.TypeName).ToList())}'";
            }

            if (duplicateTypeList.Count > 0)
            {
                errMesg = errMesg + $" There are duplicate input user type(s): '{string.Join(",", duplicateTypeList)}', please remove the duplicate input type(s).";
            }

            if (errMesg.Length > 0)
            {
                return new ValidationResult(errMesg);
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
