using CustomerApi.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Repositories
{
    public class SqlCustomerRepository : ICustomerRepository
    {
        private CustomerDbContext _context;
        public SqlCustomerRepository(CustomerDbContext context)
        {
            this._context = context;
        }

        public bool IsEmailExisting(string email)
        {
            Customer customer = this._context.Customers.Where(x => x.Email.ToUpper() == email.Trim().ToUpper()).FirstOrDefault();

            return !(customer == null);
        }

        public async Task<Customer> RegisterAsync(Customer customer)
        {
            await this._context.AddAsync(customer);
            await this._context.SaveChangesAsync();

            return customer;
        }
    }
}
