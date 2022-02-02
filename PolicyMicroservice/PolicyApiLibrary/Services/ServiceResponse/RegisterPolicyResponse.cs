using CommonLibrary;
using PolicyApiLibrary.DbModels;
using PolicyApiLibrary.ViewModels;

namespace PolicyApiLibrary.Services.ServiceResponse
{
    public class RegisterPolicyResponse : BaseResponse
    {
        public PolicyDetailViewModel VmPolicyDetail { get; set; }
        public Policy Policy { get; set; }

        public RegisterPolicyResponse()
        {
            this.VmPolicyDetail = new PolicyDetailViewModel() { };
            this.Policy = new Policy();
        }
    }
}
