using CustomerApi.Services.ServiceResponse;
using CustomerApi.ViewModels;
using System.Threading.Tasks;

namespace CustomerApi.Services
{
    public interface ICustomerService
    {
        Task<RegisterCustomerResponse> RegisterCustomerAsync(CustomerViewModel vmCstomer);
    }
}
