using PolicyApiLibrary.DbModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PolicyApiLibrary.Repositories
{
    public interface IPolicyUserTypeRepository
    {
        Task<List<PolicyUserType>> AddPolicyUserTypeListAsync(List<PolicyUserType> policyUserTypeList);
    }
}
