using PolicyApiLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
