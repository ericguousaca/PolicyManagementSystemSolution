using Microsoft.AspNetCore.SignalR;

namespace PolicyManagementWebApi.Hubs
{
    public class SearchPolicyHub : Hub
    {
        public string GetConnectionId()
        {
            return this.Context.ConnectionId;
        }
    }
}
