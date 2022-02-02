using System.Collections.Generic;

#nullable disable

namespace CustomerApi.Models
{
    public partial class EmployeeType
    {
        public EmployeeType()
        {
            Customers = new HashSet<Customer>();
        }

        public int Id { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
