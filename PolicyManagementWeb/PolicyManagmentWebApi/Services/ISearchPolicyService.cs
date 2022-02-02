using PolicyManagementWebApi.Models;
using PolicyManagementWebApi.Services.ServiceResponse;
using System.Threading.Tasks;

namespace PolicyManagementWebApi.Services
{
    public interface ISearchPolicyService
    {
        Task<GetSearchPolicyResultsResponse> GetSearchPolicyResults(string searchId);
        Task<SubmitSearchPolicyCommandResponse> SubmitSearchPolicyCommand(SearchPolicyParamModel paramsModel);
    }
}
