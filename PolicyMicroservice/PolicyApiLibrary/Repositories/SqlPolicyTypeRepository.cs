using PolicyApiLibrary.DbModels;
using System.Collections.Generic;
using System.Linq;

namespace PolicyApiLibrary.Repositories
{
    public class SqlPolicyTypeRepository : IPolicyTypeRepository
    {
        public readonly PolicyDbContext _policyDbContext;

        public SqlPolicyTypeRepository(PolicyDbContext policyDbContext)
        {
            this._policyDbContext = policyDbContext;
        }

        public PolicyType GetPolicyType(string policyTypeName)
        {
            return this._policyDbContext.PolicyTypes
                    .Where(x => x.TypeName.ToUpper() == policyTypeName.Trim()
                    .ToUpper()).FirstOrDefault();
        }

        public IEnumerable<PolicyType> GetPolicyTypes()
        {
            return this._policyDbContext.PolicyTypes.ToList();
        }
    }
}
