using CommonLibrary.Messaging.Commands;
using CommonLibrary.Messaging.Constants;
using CommonLibrary.Messaging.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PolicyApiLibrary.Models;
using PolicyApiLibrary.Services;
using PolicyApiLibrary.Services.ServiceResponse;
using PolicyApiLibrary.ViewModels;
using System;
using System.Threading.Tasks;

namespace PolicyApiLibrary.Messaging.Consumers
{
    public class SearchPolicyCommandConsumer : IConsumer<ISearchPolicyCommand>
    {
        private readonly ILogger<SearchPolicyCommandConsumer> _logger;
        private readonly IPolicyService _policyService;

        public SearchPolicyCommandConsumer(ILogger<SearchPolicyCommandConsumer> logger, IPolicyService policyService)
        {
            this._logger = logger;
            this._policyService = policyService;
        }

        public async Task Consume(ConsumeContext<ISearchPolicyCommand> context)
        {
            this._logger.LogInformation("Started consume serach command.");
            var sendToUri = new Uri($"{RabbitMqConstants.RabbitMqUri }/{RabbitMqConstants.SearchPolicyResultCommandQueue}");

            ISearchPolicyResultCommand resultCommand = new SearchPolicyResultCommand();
            try
            {
                // Get search command from Context
                ISearchPolicyCommand searchCommand = (ISearchPolicyCommand)context.Message;

                this._logger.LogInformation($"Search command: {JsonConvert.SerializeObject(searchCommand)}");

                // Build search params for Policy service search function
                SearchPolicyParamModel paramModel = new SearchPolicyParamModel();
                paramModel.SearchId = searchCommand.SearchId.ToString();
                paramModel.PolicyId = searchCommand.PolicyId;
                paramModel.PolicyType = searchCommand.PolicyType;
                paramModel.PolicyName = searchCommand.PolicyName;
                paramModel.NumberOfYears = searchCommand.NumberOfYears;
                paramModel.CompanyName = searchCommand.CompanyName;

                // Search policy by search params
                SearchPolicyDetailsResponse searchResponse = await this._policyService.SearchPolicyDetailsAsync(paramModel);

                // Process search response
                if (searchResponse.Success)
                {
                    // Build ISearchPolicyResultCommand object
                    resultCommand.Success = true;
                    resultCommand.SearchPolicyCommand = new SearchPolicyCommand
                    {
                        SearchId = searchCommand.SearchId,
                        PolicyId = searchCommand.PolicyId,
                        PolicyType = searchCommand.PolicyType,
                        PolicyName = searchCommand.PolicyName,
                        NumberOfYears = searchCommand.NumberOfYears,
                        CompanyName = searchCommand.CompanyName,
                        CommandTime = searchCommand.CommandTime,
                    };
                    resultCommand.CommandTime = DateTime.Now;

                    foreach (PolicyDetailViewModel vmDetail in searchResponse.PolicyDetails)
                    {
                        MessagePolicyDetail detail = new MessagePolicyDetail();

                        detail.Id = vmDetail.Id;
                        detail.PolicyId = vmDetail.PolicyId;
                        detail.PolicyType = vmDetail.PolicyType;
                        detail.PolicyName = vmDetail.PolicyName;
                        detail.StartDate = vmDetail.StartDate;
                        detail.DurationInYears = vmDetail.DurationInYears;
                        detail.CompanyName = vmDetail.CompanyName;
                        detail.InitialDeposit = vmDetail.InitialDeposit;
                        detail.UserTypes = vmDetail.UserTypes;
                        detail.TermsPerYear = vmDetail.TermsPerYear;
                        detail.TermAmount = vmDetail.TermAmount;
                        detail.Interest = vmDetail.Interest;
                        detail.MaturityAmount = vmDetail.MaturityAmount;

                        resultCommand.PolicyDetails.Add(detail);
                    }

                    // Send result command back to RabbitMq
                    await context.Send<ISearchPolicyResultCommand>(sendToUri, resultCommand);
                }
                else
                {
                    resultCommand.Success = false;
                    resultCommand.Message = searchResponse.PublicMessage;
                    resultCommand.CommandTime = DateTime.Now;

                    await context.Send<ISearchPolicyResultCommand>(sendToUri, resultCommand);
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                this._logger.LogError(ex, ex.Message);

                resultCommand.Success = false;
                resultCommand.Message = "An exception occurred in consuming Search Policy Command from RabbitMq, if continue see this error, please contact IT Support.";
                resultCommand.CommandTime = DateTime.Now;

                await context.Send<ISearchPolicyResultCommand>(sendToUri, resultCommand);
            }
        }
    }
}
