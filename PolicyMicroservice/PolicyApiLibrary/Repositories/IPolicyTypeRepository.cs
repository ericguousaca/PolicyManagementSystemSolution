using PolicyApiLibrary.DbModels;
using System.Collections.Generic;

namespace PolicyApiLibrary.Repositories
{
    public interface IPolicyTypeRepository
    {
        IEnumerable<PolicyType> GetPolicyTypes();

        PolicyType GetPolicyType(string policyTypeName);
    }
}
