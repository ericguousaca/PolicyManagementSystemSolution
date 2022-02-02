using CustomerApi.Controllers;
using CustomerApi.Services;
using CustomerApi.Services.ServiceResponse;
using CustomerApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CustomerApiTestProject
{
    public class CustomerControllerTests
    {
        private Mock<ILogger<CustomerController>> mockControllerLogger;
        private Mock<ICustomerService> mockCustomerService;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Customer_Register_Success_Test()
        {
            // Define a valid Customer ViewModel object
            CustomerViewModel vmCustomer = new CustomerViewModel();
            vmCustomer.CustomerId = 100;
            vmCustomer.FirstName = "Mike";
            vmCustomer.LastName = "Smith";
            vmCustomer.DateOfBirth = "12/06/1986";
            vmCustomer.Address = "123 Abc Road, Toronton, ON M2H2W9";
            vmCustomer.ContactNumber = "804-504-3946";
            vmCustomer.Email = "user1@example.com";
            vmCustomer.Salary = new decimal(3564.56);
            vmCustomer.PanCardNumber = "536474854";
            vmCustomer.EmployeeType = "salaried";
            vmCustomer.Employer = "Cognizant Technology Inc.";

            // Define a success response returned from CustomerService mock object
            RegisterCustomerResponse response = new RegisterCustomerResponse();
            response.VmCustomer = vmCustomer;
            response.Success = true;
            response.PublicMessage = "";
            response.InternalMessge = "";

            // Mock the two dependency injected service and logger
            mockControllerLogger = new Mock<ILogger<CustomerController>>();
            mockCustomerService = new Mock<ICustomerService>();

            // Set up the the method to return a valid response object
            mockCustomerService.Setup(x => x.RegisterCustomerAsync(It.IsAny<CustomerViewModel>())).ReturnsAsync(response);

            // Set up the LogInformation and LogError method
            // TODO: Below no working, need more sesearch
            // mockControllerLogger.Setup(x => x.LogInformation(It.IsAny<string>(), null));
            // mockControllerLogger.Setup(x => x.LogError(It.IsAny<string>()));

            CustomerController controller = new CustomerController(mockControllerLogger.Object, mockCustomerService.Object);

            IActionResult result = await controller.Register(vmCustomer);

            // Since success, the result can be casted as CreatedResult type
            Assert.IsInstanceOf<CreatedResult>(result);

            CreatedResult createdResult = (CreatedResult)result;

            Assert.IsInstanceOf<CustomerViewModel>(createdResult.Value);

            CustomerViewModel resultVmCustomer = (CustomerViewModel)createdResult.Value;

            Assert.AreEqual(resultVmCustomer.CustomerId, 100);
        }

        [Test]
        public async Task Customer_Register_Failed_Test()
        {
            // Define a invalid Customer ViewModel object
            CustomerViewModel vmCustomer = new CustomerViewModel();
            vmCustomer.CustomerId = 0;
            vmCustomer.FirstName = "";
            vmCustomer.LastName = "";
            vmCustomer.DateOfBirth = "12-06-1986";
            vmCustomer.Address = "123 Abc Road, Toronton, ON M2H2W9";
            vmCustomer.ContactNumber = "A804-504-3946";
            vmCustomer.Email = "user1";
            vmCustomer.Salary = new decimal(3564.56);
            vmCustomer.PanCardNumber = "536474854";
            vmCustomer.EmployeeType = "salaried";
            vmCustomer.Employer = "";

            // Define a success response returned from CustomerService mock object
            RegisterCustomerResponse response = new RegisterCustomerResponse();
            response.VmCustomer = vmCustomer;
            response.Success = false;
            response.PublicMessage = "";
            response.InternalMessge = "";

            // Mock the two dependency injected service and logger
            mockControllerLogger = new Mock<ILogger<CustomerController>>();
            mockCustomerService = new Mock<ICustomerService>();

            // Set up the the method to return a valid response object
            mockCustomerService.Setup(x => x.RegisterCustomerAsync(It.IsAny<CustomerViewModel>())).ReturnsAsync(response);

            // Set up the LogInformation and LogError method
            // TODO: Below no working, need more sesearch
            // mockControllerLogger.Setup(x => x.LogInformation(It.IsAny<string>(), null));
            // mockControllerLogger.Setup(x => x.LogError(It.IsAny<string>()));

            CustomerController controller = new CustomerController(mockControllerLogger.Object, mockCustomerService.Object);

            // Can add more errors
            controller.ModelState.AddModelError("First Name", "First name is required");
            controller.ModelState.AddModelError("Last Name", "Last name is required");

            IActionResult result = await controller.Register(vmCustomer);

            //controller.ModelState.

            // Since success, the result can be casted as BadRequestObjectResult type
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            // Can do more assert here
        }
    }
}
