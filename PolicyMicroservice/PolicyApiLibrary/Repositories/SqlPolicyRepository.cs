using Microsoft.EntityFrameworkCore;
using PolicyApiLibrary.DbModels;
using PolicyApiLibrary.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyApiLibrary.Repositories
{
    public class SqlPolicyRepository : IPolicyRepository
    {
        public readonly PolicyDbContext _policyDbContext;
        private readonly IPolicyUserTypeRepository _policyUserTypeRepo;

        public SqlPolicyRepository(PolicyDbContext policyDbContext, IPolicyUserTypeRepository policyUserTypeRepo)
        {
            this._policyDbContext = policyDbContext;
            this._policyUserTypeRepo = policyUserTypeRepo;
        }

        public async Task<Policy> AddPolicyAsync(Policy policy, List<UserType> userTypeList)
        {
            await this._policyDbContext.AddAsync(policy);
            await this._policyDbContext.SaveChangesAsync();

            List<PolicyUserType> policyUserTypeList = new List<PolicyUserType> { };

            foreach (UserType userType in userTypeList)
            {
                PolicyUserType policyUserType = new PolicyUserType();
                policyUserType.PolicyId = policy.Id;
                policyUserType.UserTypeId = userType.Id;

                policyUserTypeList.Add(policyUserType);
            }

            await this._policyUserTypeRepo.AddPolicyUserTypeListAsync(policyUserTypeList);

            policy = this._policyDbContext.Policies
                            .Include(x => x.PolicyUserTypes)
                            .Include(x => x.PolicyType)
                            .Where(x => x.Id == policy.Id).FirstOrDefault();

            return policy;
        }

        public async Task<IEnumerable<Policy>> GetAllPoliciesAsync()
        {
            var query = this._policyDbContext.Policies
                            .Include(x => x.PolicyType)
                            .Include(x => x.PolicyUserTypes)
                            .ThenInclude(y => y.UserType);

            return await query
                    .OrderBy(x => x.Id)
                    .ToListAsync();
        }

        public async Task<Policy> GetLastPolicyByPolicyTypeIdAsync(string policyTypeId)
        {
            return await this._policyDbContext.Policies
                                .Where(x => x.PolicyTypeId == policyTypeId)
                                .OrderByDescending(x => x.Id)
                                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Policy>> SearchPolicy(SearchPolicyParamModel paramModel)
        {
            paramModel.PolicyType = paramModel.PolicyType.Trim();
            paramModel.CompanyName = paramModel.CompanyName.Trim();
            paramModel.PolicyId = paramModel.PolicyId.Trim();
            paramModel.PolicyName = paramModel.PolicyName.Trim();

            IQueryable<Policy> policies = this._policyDbContext.Policies
                                    .Include(x => x.PolicyType)
                                    .Include(x => x.PolicyUserTypes)
                                    .ThenInclude(y => y.UserType)
                                    .OrderBy(x => x.Id);

            if (!string.IsNullOrEmpty(paramModel.PolicyType))
            {
                policies = policies.Where(x => x.PolicyType.TypeName.ToUpper() == paramModel.PolicyType.ToUpper());
            }

            if (paramModel.NumberOfYears > 0)
            {
                policies = policies.Where(x => x.DurationInYear == paramModel.NumberOfYears);
            }

            if (!string.IsNullOrEmpty(paramModel.CompanyName))
            {
                policies = policies.Where(x => x.ComapnyName.ToUpper().Contains(paramModel.CompanyName.ToUpper()));
            }

            if (!string.IsNullOrEmpty(paramModel.PolicyId))
            {
                policies = policies.Where(x => x.PolicyId.ToUpper().Contains(paramModel.PolicyId.ToUpper()));
            }

            if (!string.IsNullOrEmpty(paramModel.PolicyName))
            {
                policies = policies.Where(x => x.PolicyName.ToUpper().Contains(paramModel.PolicyName.ToUpper()));
            }

            return await policies.ToListAsync();
        }

        public bool IsPolicyNameExisting(string policyName)
        {
            Policy policy = this._policyDbContext.Policies.Where(x => x.PolicyName.ToUpper() == policyName.Trim().ToUpper()).FirstOrDefault();

            if (policy == null)
            {
                return false;
            }

            return true;
        }


    }
}
