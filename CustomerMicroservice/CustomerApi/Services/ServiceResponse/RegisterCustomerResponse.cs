using CommonLibrary;
using CustomerApi.ViewModels;

namespace CustomerApi.Services.ServiceResponse
{
    public class RegisterCustomerResponse : BaseResponse
    {
        public CustomerViewModel VmCustomer { get; set; }

        public RegisterCustomerResponse()
        {
            this.VmCustomer = new CustomerViewModel();
        }
    }
}
