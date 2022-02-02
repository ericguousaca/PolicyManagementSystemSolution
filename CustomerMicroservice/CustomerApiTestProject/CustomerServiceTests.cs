using CustomerApi.Models;
using CustomerApi.Repositories;
using CustomerApi.Services;
using CustomerApi.Services.ServiceResponse;
using CustomerApi.ViewModels;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerApiTestProject
{
    // TODO: Will add the Service testing by mocking the next level Repository classes
    public class CustomerServiceTests
    {
        private Mock<ILogger<CustomerService>> mockServiceLogger = new Mock<ILogger<CustomerService>>();
        private Mock<ICustomerRepository> mockCustomerRepo = new Mock<ICustomerRepository>();
        private Mock<IEmployeeTypeRepository> mockEmployeeTypeRepo = new Mock<IEmployeeTypeRepository>();

        [SetUp]
        public void Setup()
        {
            List<EmployeeType> employeeTypes = new List<EmployeeType>();
            employeeTypes.Add(new EmployeeType { Id = 1, TypeName = "salaried" });
            employeeTypes.Add(new EmployeeType { Id = 2, TypeName = "self-employed" });
            this.mockEmployeeTypeRepo.Setup(x => x.GetEmployeeTypes()).Returns(employeeTypes);
        }

        [Test]
        public async Task RegisterCustomerAsync_Success_Test()
        {
            int mockCustomerId = 100;
            // Define a valid Customer ViewModel object
            CustomerViewModel vmCustomer = new CustomerViewModel();
            vmCustomer.CustomerId = 0;
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

            Customer newCustomer = new Customer();
            newCustomer.Id = mockCustomerId;

            this.mockCustomerRepo.Setup(x => x.IsEmailExisting(It.IsAny<string>())).Returns(false);
            this.mockCustomerRepo.Setup(x => x.RegisterAsync(It.IsAny<Customer>())).ReturnsAsync(newCustomer);

            CustomerService customerService = new CustomerService(this.mockServiceLogger.Object, this.mockCustomerRepo.Object, this.mockEmployeeTypeRepo.Object);
            RegisterCustomerResponse response = await customerService.RegisterCustomerAsync(vmCustomer);

            Assert.AreEqual(response.Success, true);
            Assert.AreEqual(response.VmCustomer.CustomerId, mockCustomerId);
        }

        [Test]
        public async Task RegisterCustomerAsync_EmailExisting_Failed_Test()
        {
            int mockCustomerId = 100;
            // Define a valid Customer ViewModel object
            CustomerViewModel vmCustomer = new CustomerViewModel();
            vmCustomer.CustomerId = 0;
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

            Customer newCustomer = new Customer();
            newCustomer.Id = mockCustomerId;

            this.mockCustomerRepo.Setup(x => x.IsEmailExisting(It.IsAny<string>())).Returns(true);
            this.mockCustomerRepo.Setup(x => x.RegisterAsync(It.IsAny<Customer>())).ReturnsAsync(newCustomer);

            CustomerService customerService = new CustomerService(this.mockServiceLogger.Object, this.mockCustomerRepo.Object, this.mockEmployeeTypeRepo.Object);
            RegisterCustomerResponse response = await customerService.RegisterCustomerAsync(vmCustomer);

            Assert.AreEqual(response.Success, false);
            Assert.AreEqual(response.VmCustomer.CustomerId, 0);
            Assert.AreNotEqual(response.PublicMessage, "");
            Assert.AreNotEqual(response.InternalMessge, "");
        }

        [Test]
        public async Task RegisterCustomerAsync_InvalidDateOfBirthFormat_Failed_Test()
        {
            int mockCustomerId = 100;
            // Define a valid Customer ViewModel object
            CustomerViewModel vmCustomer = new CustomerViewModel();
            vmCustomer.CustomerId = 0;
            vmCustomer.FirstName = "Mike";
            vmCustomer.LastName = "Smith";
            vmCustomer.DateOfBirth = "15/14/1986";
            vmCustomer.Address = "123 Abc Road, Toronton, ON M2H2W9";
            vmCustomer.ContactNumber = "804-504-3946";
            vmCustomer.Email = "user1@example.com";
            vmCustomer.Salary = new decimal(3564.56);
            vmCustomer.PanCardNumber = "536474854";
            vmCustomer.EmployeeType = "salaried";
            vmCustomer.Employer = "Cognizant Technology Inc.";

            Customer newCustomer = new Customer();
            newCustomer.Id = mockCustomerId;

            this.mockCustomerRepo.Setup(x => x.IsEmailExisting(It.IsAny<string>())).Returns(false);
            this.mockCustomerRepo.Setup(x => x.RegisterAsync(It.IsAny<Customer>())).ReturnsAsync(newCustomer);

            CustomerService customerService = new CustomerService(this.mockServiceLogger.Object, this.mockCustomerRepo.Object, this.mockEmployeeTypeRepo.Object);
            RegisterCustomerResponse response = await customerService.RegisterCustomerAsync(vmCustomer);

            Assert.AreEqual(response.Success, false);
            Assert.AreEqual(response.VmCustomer.CustomerId, 0);
            Assert.AreNotEqual(response.PublicMessage, "");
            Assert.AreNotEqual(response.InternalMessge, "");
        }
    }
}
