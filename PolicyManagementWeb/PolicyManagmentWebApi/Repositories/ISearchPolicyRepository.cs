using CommonLibrary.Messaging.Commands;
using System.Threading.Tasks;

namespace PolicyManagementWebApi.Repositories
{
    public interface ISearchPolicyRepository
    {
        Task AddSearchPolicyResultCommand(SearchPolicyResultCommand resultCommand);
        Task<ISearchPolicyResultCommand> GetSearchPolicyResultCommand(string searchId);
    }
}
