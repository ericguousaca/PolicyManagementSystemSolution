using CustomerApi.Models;
using System.Collections.Generic;

namespace CustomerApi.Repositories
{
    public interface IEmployeeTypeRepository
    {
        IEnumerable<EmployeeType> GetEmployeeTypes();
    }
}
