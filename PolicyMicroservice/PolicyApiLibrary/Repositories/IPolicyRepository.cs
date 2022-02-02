using PolicyApiLibrary.DbModels;
using PolicyApiLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PolicyApiLibrary.Repositories
{
    public interface IPolicyRepository
    {
        Task<Policy> AddPolicyAsync(Policy policy, List<UserType> userTypeList);

        Task<IEnumerable<Policy>> GetAllPoliciesAsync();

        Task<Policy> GetLastPolicyByPolicyTypeIdAsync(string policyTypeId);
        bool IsPolicyNameExisting(string policyName);
        Task<IEnumerable<Policy>> SearchPolicy(SearchPolicyParamModel paramModel);
    }
}
