using CustomerApi.Models;
using System.Threading.Tasks;

namespace CustomerApi.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> RegisterAsync(Customer customer);

        bool IsEmailExisting(string email);
    }
}
