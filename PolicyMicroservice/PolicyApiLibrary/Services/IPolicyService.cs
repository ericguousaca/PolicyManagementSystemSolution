using PolicyApiLibrary.DbModels;
using PolicyApiLibrary.Models;
using PolicyApiLibrary.Services.ServiceResponse;
using PolicyApiLibrary.ViewModels;
using System.Threading.Tasks;

namespace PolicyApiLibrary.Services
{
    public interface IPolicyService
    {
        Task<RegisterPolicyResponse> RegisterPolicyAsync(PolicyViewModel vmPolicy);

        Task<GetAllPolicyDetailsResponse> GetAllPolicyDetailsAsync(int skip = -1, int pageSize = 0, string sortBy = "Id", string sortDirection = "ASC");

        string GeneratePolicyRegitserHtmlMessage(Policy policy, string templateFilePath);
        Task<SearchPolicyDetailsResponse> SearchPolicyDetailsAsync(SearchPolicyParamModel paramModel);
    }
}
