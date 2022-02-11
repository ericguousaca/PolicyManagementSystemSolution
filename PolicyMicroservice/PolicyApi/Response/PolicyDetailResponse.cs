using PolicyApiLibrary.ViewModels;
using System.Collections.Generic;

namespace PolicyApi.Response
{
    public class PolicyDetailResponse
    {
        public int TotalCount { get; set; }
        public List<PolicyDetailViewModel> PolicyDetails { get; set; }

        public PolicyDetailResponse()
        {
            this.TotalCount = 0;
            this.PolicyDetails = new List<PolicyDetailViewModel> { };
        }
    }
}
