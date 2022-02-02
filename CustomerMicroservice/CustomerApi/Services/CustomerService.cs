using CustomerApi.Models;
using CustomerApi.Repositories;
using CustomerApi.Services.ServiceResponse;
using CustomerApi.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly ICustomerRepository _customerRepo;
        private readonly IEmployeeTypeRepository _employeeTypeRepo;

        public CustomerService(ILogger<CustomerService> logger, ICustomerRepository customerRepo, IEmployeeTypeRepository employeeTypeRepo)
        {
            this._logger = logger;
            this._customerRepo = customerRepo;
            this._employeeTypeRepo = employeeTypeRepo;
        }
        public async Task<RegisterCustomerResponse> RegisterCustomerAsync(CustomerViewModel vmCustomer)
        {
            this._logger.LogInformation("Start register customer async.");
            RegisterCustomerResponse response = new RegisterCustomerResponse();
            response.VmCustomer = vmCustomer;

            try
            {
                if (_customerRepo.IsEmailExisting(vmCustomer.Email) == false)
                {
                    DateTime birthday = DateTime.MinValue;

                    if (!DateTime.TryParseExact(vmCustomer.DateOfBirth, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out birthday))
                    {
                        response.Success = false;
                        response.PublicMessage = "Date of birth format error.";
                        response.InternalMessge = "Date of birth format error.";

                        return response;
                    }

                    // Validate employee type name with items in EmployeeType table
                    IEnumerable<EmployeeType> employeeTypes = this._employeeTypeRepo.GetEmployeeTypes();
                    EmployeeType selectedEmployeeType = employeeTypes.FirstOrDefault(x => x.TypeName == vmCustomer.EmployeeType);
                    if (selectedEmployeeType == null)
                    {
                        response.Success = false;
                        response.PublicMessage = "Invalid employee type.";
                        response.InternalMessge = $"Invalida employee type, the EmployeeType table has not such type: {vmCustomer.EmployeeType}";

                        return response;
                    }

                    // Convert to database model
                    Customer newCustomer = new Customer();
                    newCustomer.FirstName = vmCustomer.FirstName.Trim();
                    newCustomer.LastName = vmCustomer.LastName.Trim();
                    newCustomer.DateOfBirth = birthday;
                    newCustomer.Address = vmCustomer.Address.Trim();
                    newCustomer.ContactNo = vmCustomer.ContactNumber;
                    newCustomer.Email = vmCustomer.Email;
                    newCustomer.Salary = vmCustomer.Salary;
                    newCustomer.PanNo = vmCustomer.PanCardNumber.Trim();
                    newCustomer.EmployeeTypeId = selectedEmployeeType.Id;
                    newCustomer.Employer = vmCustomer.Employer.Trim();

                    newCustomer = await _customerRepo.RegisterAsync(newCustomer);

                    response.VmCustomer.CustomerId = newCustomer.Id;
                    response.Success = true;

                    return response;
                }
                else
                {
                    response.Success = false;
                    response.PublicMessage = "Email is existing in system.";
                    response.InternalMessge = "Email is existing in table Customer.";
                }

                this._logger.LogInformation("Finished register customer async.");

                return response;
            }
            catch (Exception ex)
            {
                response.PublicMessage = "Had exception in Registering customer, please try again, if continue see this error, please contact IT support.";
                response.InternalMessge = ex.Message;
                response.Exception = ex;

                this._logger.LogError(ex, "Register customer async has exception");

                return response;
            }
        }
    }
}
