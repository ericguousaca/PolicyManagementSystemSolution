using CustomerApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace CustomerApi.Repositories
{
    public class SqlEmployeeTypeRepository : IEmployeeTypeRepository
    {
        private readonly CustomerDbContext _context;

        public SqlEmployeeTypeRepository(CustomerDbContext context)
        {
            this._context = context;
        }
        public IEnumerable<EmployeeType> GetEmployeeTypes()
        {
            return this._context.EmployeeTypes.ToList();
        }
    }
}
