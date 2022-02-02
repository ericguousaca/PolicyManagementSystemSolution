using CommonLibrary;
using PolicyApiLibrary.Models;
using PolicyApiLibrary.ViewModels;
using System.Collections.Generic;

namespace PolicyApiLibrary.Services.ServiceResponse
{
    public class SearchPolicyDetailsResponse : BaseResponse
    {
        public SearchPolicyParamModel SearchParamModel { get; set; }
        public IEnumerable<PolicyDetailViewModel> PolicyDetails { get; set; }

        public SearchPolicyDetailsResponse()
        {
            this.SearchParamModel = new SearchPolicyParamModel();
            this.PolicyDetails = new List<PolicyDetailViewModel> { };
        }
    }
}
