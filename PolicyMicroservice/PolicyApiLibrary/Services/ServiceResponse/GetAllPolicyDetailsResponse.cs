using CommonLibrary;
using PolicyApiLibrary.ViewModels;
using System.Collections.Generic;

namespace PolicyApiLibrary.Services.ServiceResponse
{
    public class GetAllPolicyDetailsResponse : BaseResponse
    {
        public IEnumerable<PolicyDetailViewModel> PolicyDetails { get; set; }
        public int TotalCount { get; set; }

        public GetAllPolicyDetailsResponse()
        {
            this.PolicyDetails = new List<PolicyDetailViewModel> { };
            this.TotalCount = 0;
        }
    }
}
