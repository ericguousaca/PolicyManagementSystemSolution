using PolicyApiLibrary.DbModels;
using PolicyApiLibrary.ViewModels;
using System;
using System.Linq;

namespace PolicyApiLibrary.Builders
{
    public class PolicyDetailBuilder : IPolicyDetailBuilder
    {
        private Policy _policy;
        private PolicyDetailViewModel _policyDetail;
        public PolicyDetailBuilder()
        {
            this._policy = new Policy();
            this._policyDetail = new PolicyDetailViewModel();
        }

        public void SetupBuilder(Policy policy)
        {
            this._policy = policy;
            this._policyDetail = new PolicyDetailViewModel();
        }

        public void BuildMainBodySection()
        {
            this._policyDetail.Id = this._policy.Id;
            this._policyDetail.PolicyId = this._policy.PolicyId;
            this._policyDetail.PolicyName = this._policy.PolicyName;
            this._policyDetail.StartDate = this._policy.StartDate.ToString("dd'/'MM'/'yyyy");
            this._policyDetail.DurationInYears = this._policy.DurationInYear;
            this._policyDetail.CompanyName = this._policy.ComapnyName;
            this._policyDetail.InitialDeposit = this._policy.InitialDeposit;
            this._policyDetail.TermsPerYear = this._policy.TermsPerYear;
            this._policyDetail.TermAmount = this._policy.TermAmount;
            this._policyDetail.Interest = this._policy.Interest;
        }

        public void BuildUserTypeSection()
        {
            this._policyDetail.UserTypes = this._policy.PolicyUserTypes.Select(x => x.UserType.TypeName).ToList();
        }

        public void BuildPolicyTypeSection()
        {
            this._policyDetail.PolicyType = this._policy.PolicyType.TypeName;
        }

        public void BuildMaturityAmountSection()
        {
            this._policyDetail.MaturityAmount = this._policy.InitialDeposit
                                        + (this._policy.DurationInYear * this._policy.TermsPerYear * this._policy.TermAmount)
                                        + (this._policy.DurationInYear * this._policy.TermsPerYear * this._policy.TermAmount) * Convert.ToDecimal(this._policy.Interest);
        }

        public PolicyDetailViewModel GetPolicyDetail()
        {
            return this._policyDetail;
        }
    }
}
