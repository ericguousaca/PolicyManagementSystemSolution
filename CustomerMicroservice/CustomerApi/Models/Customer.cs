using System;

#nullable disable

namespace CustomerApi.Models
{
    public partial class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public decimal Salary { get; set; }
        public string PanNo { get; set; }
        public int EmployeeTypeId { get; set; }
        public string Employer { get; set; }

        public virtual EmployeeType EmployeeType { get; set; }
    }
}
