using PolicyApiLibrary.DbModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PolicyApiLibrary.Repositories
{
    public class SqlPolicyUserTypeRepository : IPolicyUserTypeRepository
    {
        public readonly PolicyDbContext _policyDbContext;

        public SqlPolicyUserTypeRepository(PolicyDbContext policyDbContext)
        {
            this._policyDbContext = policyDbContext;
        }

        public async Task<List<PolicyUserType>> AddPolicyUserTypeListAsync(List<PolicyUserType> policyUserTypeList)
        {
            await this._policyDbContext.PolicyUserTypes.AddRangeAsync(policyUserTypeList);
            await this._policyDbContext.SaveChangesAsync();

            return policyUserTypeList;
        }
    }
}
