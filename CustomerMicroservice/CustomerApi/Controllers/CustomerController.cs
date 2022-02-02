using CustomerApi.Services;
using CustomerApi.Services.ServiceResponse;
//using CustomerApi.Models;
using CustomerApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Controllers
{
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            this._logger = logger;
            this._customerService = customerService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(CustomerViewModel vmCustomer)
        {
            try
            {
                this._logger.LogInformation("Start register customer.");
                if (ModelState.IsValid)
                {
                    RegisterCustomerResponse response = await this._customerService.RegisterCustomerAsync(vmCustomer);

                    if (response.Success)
                    {
                        this._logger.LogInformation("Finish register customer success.");
                        return Created("~api/v1.0/customer/register", response.VmCustomer);
                    }
                    else
                    {
                        this._logger.LogError($"Finish register customer with error: {response.InternalMessge}");
                        return StatusCode(StatusCodes.Status500InternalServerError, response.PublicMessage);
                    }
                }
                else
                {
                    string modelStateErrorStr = string.Join("; ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    this._logger.LogError($"Finish register customer with validation error: {modelStateErrorStr}");
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An exception happened, please try again. Please contact IT Support if continue see the error");
            }
        }
    }
}
