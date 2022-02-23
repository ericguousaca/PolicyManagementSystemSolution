using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using PolicyManagementWebApi.Constants;
using PolicyManagementWebApi.Hubs;
using PolicyManagementWebApi.Models;
using PolicyManagementWebApi.Services;
using PolicyManagementWebApi.Services.ServiceResponse;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PolicyManagementWebApi.Controllers
{
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class PolicySearchController : ControllerBase
    {
        private readonly ILogger<PolicySearchController> _logger;
        private readonly ISearchPolicyService _policySearchService;
        private readonly IHubContext<SearchPolicyHub> _hubContext;

        public object StaticVaribles { get; private set; }

        public PolicySearchController(ILogger<PolicySearchController> logger, ISearchPolicyService policySearchService, IHubContext<SearchPolicyHub> hubContext)
        {
            this._logger = logger;
            this._policySearchService = policySearchService;
            this._hubContext = hubContext;


        }

        [HttpPost]
        [Route("SubmitSearchPolicyCommand")]
        public async Task<ActionResult<SubmittedSearchPolicyParamModel>> SubmitSearchPolicyCommand(SearchPolicyParamModel paramsModel)
        {
            try
            {
                this._logger.LogInformation("Started submit search policy command to RabbitMq.");

                SubmitSearchPolicyCommandResponse response = await this._policySearchService.SubmitSearchPolicyCommand(paramsModel);

                this._logger.LogInformation("Success submitted the search policy command to RabbitMq.");

                if (response.Success)
                {
                    await this._hubContext.Clients.Client(paramsModel.SearchId).SendAsync("SearchPolicyAsync", SearchPolicyAsyncStatus.SEARCH_SENT, null);


                    this._logger.LogInformation("Success sent the SEARCH_SENT status to Client asyn by SignalR.");
                    return Ok(response.SubmittedModel);
                }
                else
                {
                    await this._hubContext.Clients.Client(paramsModel.SearchId).SendAsync("SearchPolicyAsync", SearchPolicyAsyncStatus.SEARCH_ERROR, null);
                    this._logger.LogError("Failed submitting the search policy command to RabbitMq.");
                    return StatusCode(StatusCodes.Status500InternalServerError, response.PublicMessage);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);

                await this._hubContext.Clients.Client(paramsModel.SearchId).SendAsync("SearchPolicyAsync", SearchPolicyAsyncStatus.SEARCH_ERROR, null);
                return StatusCode(StatusCodes.Status500InternalServerError, "An exception happened, please try again. Please contact IT Support if continue see the error");
            }
        }

        [HttpGet]
        [Route("GetSearchPolicyResults/{searchId}")]
        public async Task<ActionResult<IEnumerable<SearchPolicyResultModel>>> GetSearchPolicyResults(string searchId)
        {
            try
            {
                this._logger.LogInformation("Start get Search Reults from the storge");

                GetSearchPolicyResultsResponse response = await this._policySearchService.GetSearchPolicyResults(searchId);

                if (response.Success)
                {
                    return Ok(response.SearchPolicyResults);
                }
                else
                {
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
