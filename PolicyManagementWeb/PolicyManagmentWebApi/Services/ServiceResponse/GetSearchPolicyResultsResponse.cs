using CommonLibrary;
using PolicyManagementWebApi.Models;
using System.Collections.Generic;

namespace PolicyManagementWebApi.Services.ServiceResponse
{
    public class GetSearchPolicyResultsResponse : BaseResponse
    {
        public IEnumerable<SearchPolicyResultModel> SearchPolicyResults { get; set; }

        public GetSearchPolicyResultsResponse()
        {
            this.SearchPolicyResults = new List<SearchPolicyResultModel> { };
        }
    }
}
