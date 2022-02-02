using CommonLibrary.Messaging.Commands;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using PolicyManagementWebApi.Constants;
using PolicyManagementWebApi.Hubs;
using PolicyManagementWebApi.Repositories;
using System;
using System.Threading.Tasks;

namespace PolicyManagementWebApi.Messaging.Consumers
{
    public class SearchPolicyResultCommandConsumer : IConsumer<ISearchPolicyResultCommand>
    {
        private readonly ILogger<SearchPolicyResultCommandConsumer> _logger;
        private readonly ISearchPolicyRepository _searchPolicyRepo;
        private readonly IHubContext<SearchPolicyHub> _hubContext;

        public SearchPolicyResultCommandConsumer(ILogger<SearchPolicyResultCommandConsumer> logger, ISearchPolicyRepository searchPolicyRepo, IHubContext<SearchPolicyHub> hubContext)
        {
            this._logger = logger;
            this._searchPolicyRepo = searchPolicyRepo;
            this._hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<ISearchPolicyResultCommand> context)
        {
            this._logger.LogInformation("Started consume serach result command.");

            // Get search result command from Context
            ISearchPolicyResultCommand resultCommand = (ISearchPolicyResultCommand)context.Message;
            string connectionId = resultCommand.SearchPolicyCommand.SearchId;

            try
            {
                System.Threading.Thread.Sleep(5000);

                await this._hubContext.Clients.Client(connectionId).SendAsync("SearchPolicyAsync", SearchPolicyAsyncStatus.RESULT_RECEIVED, null);

                SearchPolicyResultCommand mongoResultCommand = new SearchPolicyResultCommand();
                mongoResultCommand._id = Guid.NewGuid().ToString();
                mongoResultCommand.Success = resultCommand.Success;
                mongoResultCommand.Message = resultCommand.Message;
                mongoResultCommand.CommandTime = resultCommand.CommandTime;
                mongoResultCommand.SearchPolicyCommand = resultCommand.SearchPolicyCommand;
                mongoResultCommand.PolicyDetails = resultCommand.PolicyDetails;

                System.Threading.Thread.Sleep(5000);
                await this._searchPolicyRepo.AddSearchPolicyResultCommand(mongoResultCommand);

                await this._hubContext.Clients.Client(connectionId).SendAsync("SearchPolicyAsync", SearchPolicyAsyncStatus.RESULT_AVAILABLE, mongoResultCommand.PolicyDetails);
            }
            catch (Exception ex)
            {
                // Handle exception
                this._logger.LogError(ex, ex.Message);

                await this._hubContext.Clients.Client(connectionId).SendAsync("SearchPolicyAsync", SearchPolicyAsyncStatus.SEARCH_ERROR, null);
            }
        }
    }
}
