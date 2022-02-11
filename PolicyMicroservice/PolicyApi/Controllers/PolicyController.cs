using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PolicyApi.Response;
using PolicyApiLibrary.Models;
using PolicyApiLibrary.Services;
using PolicyApiLibrary.Services.ServiceResponse;
using PolicyApiLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyApi.Controllers
{
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly ILogger<PolicyController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IPolicyService _policyService;


        public PolicyController(ILogger<PolicyController> logger, IWebHostEnvironment hostingEnvironment, IPolicyService policyService)
        {
            this._logger = logger;
            this._hostingEnvironment = hostingEnvironment;
            this._policyService = policyService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(PolicyViewModel vmPolicy)
        {
            try
            {
                this._logger.LogInformation("Started register policy.");
                if (ModelState.IsValid)
                {
                    RegisterPolicyResponse response = new RegisterPolicyResponse();

                    response = await this._policyService.RegisterPolicyAsync(vmPolicy);
                    //response.Success = true;
                    //response.Policy = new PolicyApiLibrary.DbModels.Policy();

                    if (response.Success)
                    {
                        string templateFilePath = this._hostingEnvironment.ContentRootPath + @"\HtmlTemplates\RegisterPolicySuccessEmailTemplate.html";
                        string htmlMessage = this._policyService.GeneratePolicyRegitserHtmlMessage(response.Policy, templateFilePath);
                        this._logger.LogInformation("Registered policy success.");
                        return Created("~api/v1.0/policy/register", htmlMessage);
                    }
                    else
                    {
                        this._logger.LogError(response.InternalMessge);
                        return BadRequest(response.PublicMessage);
                    }
                }
                else
                {
                    string modelStateErrorStr = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                    this._logger.LogError($"Register customer with validation error: {modelStateErrorStr}");
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An exception happened, please try again. Please contact IT Support if continue see the error");
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<PolicyDetailResponse>> GetAll(int skip = -1, int pageSize = 0, string sortBy = "Id", string sortDirection = "ASC")
        {
            try
            {
                this._logger.LogInformation("Started get all policies.");

                GetAllPolicyDetailsResponse response = await this._policyService.GetAllPolicyDetailsAsync(skip, pageSize, sortBy, sortDirection);

                if (response.Success)
                {
                    this._logger.LogInformation("Got all policies success.");

                    PolicyDetailResponse res = new PolicyDetailResponse();
                    res.TotalCount = response.TotalCount;
                    res.PolicyDetails = response.PolicyDetails.ToList();

                    return Ok(res);
                }
                else
                {
                    this._logger.LogError($"Got all policies with error: {response.InternalMessge}");
                    return StatusCode(StatusCodes.Status500InternalServerError, response.PublicMessage);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "An exception happened, please try again. Please contact IT Support if continue see the error");
            }
        }

        [HttpPost]
        [Route("Searches")]
        public async Task<ActionResult<List<PolicyDetailViewModel>>> SearchPolicy(SearchPolicyParamModel searchParamModel)
        {
            try
            {
                this._logger.LogInformation("Started search policies.");

                SearchPolicyDetailsResponse response = await this._policyService.SearchPolicyDetailsAsync(searchParamModel);

                if (response.Success)
                {
                    this._logger.LogInformation("Finished search policies success.");
                    return Ok(response.PolicyDetails);
                }
                else
                {
                    this._logger.LogError($"Finished search policies by {JsonConvert.SerializeObject(response.SearchParamModel)} with error: {response.InternalMessge}");
                    return StatusCode(StatusCodes.Status500InternalServerError, response.PublicMessage);
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
