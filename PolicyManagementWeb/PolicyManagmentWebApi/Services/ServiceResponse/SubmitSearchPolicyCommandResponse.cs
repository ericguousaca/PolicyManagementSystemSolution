using CommonLibrary;
using PolicyManagementWebApi.Models;

namespace PolicyManagementWebApi.Services.ServiceResponse
{
    public class SubmitSearchPolicyCommandResponse : BaseResponse
    {
        public SubmittedSearchPolicyParamModel SubmittedModel { get; set; }

        public SubmitSearchPolicyCommandResponse()
        {
            this.SubmittedModel = new SubmittedSearchPolicyParamModel();
        }
    }
}
