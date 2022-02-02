using CommonLibrary.Messaging.Commands;
using CommonLibrary.Messaging.Constants;
using CommonLibrary.Messaging.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using PolicyManagementWebApi.Models;
using PolicyManagementWebApi.Repositories;
using PolicyManagementWebApi.Services.ServiceResponse;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PolicyManagementWebApi.Services
{
    public class SearchPolicyService : ISearchPolicyService
    {
        private readonly ILogger<SearchPolicyService> _logger;
        private readonly IBusControl _busControl;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISearchPolicyRepository _searchPolicyRepo;

        public SearchPolicyService(ILogger<SearchPolicyService> logger, IBusControl busControl, IPublishEndpoint publishEndpoint, ISearchPolicyRepository searchPolicyRepo)
        {
            this._logger = logger;
            this._busControl = busControl;
            this._publishEndpoint = publishEndpoint;
            this._searchPolicyRepo = searchPolicyRepo;
        }

        public async Task<SubmitSearchPolicyCommandResponse> SubmitSearchPolicyCommand(SearchPolicyParamModel paramsModel)
        {
            SubmitSearchPolicyCommandResponse response = new SubmitSearchPolicyCommandResponse();

            try
            {
                DateTime now = DateTime.Now;

                ISearchPolicyCommand searchCommand = new SearchPolicyCommand
                {
                    SearchId = paramsModel.SearchId,
                    PolicyType = paramsModel.PolicyType,
                    NumberOfYears = paramsModel.NumberOfYears,
                    CompanyName = paramsModel.CompanyName,
                    PolicyId = paramsModel.PolicyId,
                    PolicyName = paramsModel.PolicyName,
                    CommandTime = now
                };

                var sendToUri = new Uri($"{RabbitMqConstants.RabbitMqUri }/{RabbitMqConstants.SearchPolicyCommandQueue}");
                var endPoint = await _busControl.GetSendEndpoint(sendToUri);
                await endPoint.Send<ISearchPolicyCommand>(searchCommand);

                SubmittedSearchPolicyParamModel submittedModel = new SubmittedSearchPolicyParamModel();

                submittedModel.SearchId = paramsModel.SearchId;
                submittedModel.PolicyType = paramsModel.PolicyType;
                submittedModel.NumberOfYears = paramsModel.NumberOfYears;
                submittedModel.CompanyName = paramsModel.CompanyName;
                submittedModel.PolicyId = paramsModel.PolicyId;
                submittedModel.PolicyName = paramsModel.PolicyName;
                submittedModel.SubmittedTime = now;

                response.Success = true;
                response.SubmittedModel = submittedModel;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.PublicMessage = "An exception occured, please try again, if continue see the message, please contact IT Support.";
                response.InternalMessge = ex.Message;
                response.Exception = ex;

                return response;
            }
        }

        public async Task<GetSearchPolicyResultsResponse> GetSearchPolicyResults(string searchId)
        {
            GetSearchPolicyResultsResponse response = new GetSearchPolicyResultsResponse();

            try
            {
                ISearchPolicyResultCommand resultCommand = await this._searchPolicyRepo.GetSearchPolicyResultCommand(searchId);

                if (resultCommand == null)
                {
                    response.Success = false;
                    response.PublicMessage = $"Could not find Policy Search Results by Search Id: {searchId}. Please try again.";
                    response.InternalMessge = $"Could not find the Policy Search Command object by Search Id: {searchId} from storage, maybe the Search Result command had not been consumed and saved.";

                    return response;
                }

                List<SearchPolicyResultModel> searchResults = new List<SearchPolicyResultModel>();
                foreach (MessagePolicyDetail policyDetail in resultCommand.PolicyDetails)
                {
                    SearchPolicyResultModel result = new SearchPolicyResultModel();

                    result.Id = policyDetail.Id;
                    result.PolicyId = policyDetail.PolicyId;
                    result.PolicyType = policyDetail.PolicyType;
                    result.PolicyName = policyDetail.PolicyName;
                    result.StartDate = policyDetail.StartDate;
                    result.DurationInYears = policyDetail.DurationInYears;
                    result.CompanyName = policyDetail.CompanyName;
                    result.InitialDeposit = policyDetail.InitialDeposit;
                    result.UserTypes = policyDetail.UserTypes;
                    result.TermsPerYear = policyDetail.TermsPerYear;
                    result.TermAmount = policyDetail.TermAmount;
                    result.Interest = policyDetail.Interest;
                    result.MaturityAmount = policyDetail.MaturityAmount;

                    searchResults.Add(result);
                }

                response.Success = true;
                response.SearchPolicyResults = searchResults;

                return await Task.FromResult(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.PublicMessage = "An exception occured, please try again, if continue see the message, please contact IT Support.";
                response.InternalMessge = ex.Message;
                response.Exception = ex;

                return response;
            }
        }
    }
}
