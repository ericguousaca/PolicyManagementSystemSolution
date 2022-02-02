using CustomerApi.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerApi.ViewModels
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "First name required.")]
        [MaxLength(50, ErrorMessage = "First name max length is 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name required.")]
        [MaxLength(50, ErrorMessage = "Last name max length is 50 characters.")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$", ErrorMessage = "Date of birth shall be the format of dd/mm/yyyy.")]
        [ValidDateString]
        public string DateOfBirth { get; set; }

        [MaxLength(300, ErrorMessage = "Address max length is 300 characters.")]
        public string Address { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number.")]
        public string ContactNumber { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter valid email address.")]
        public string Email { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Salary must be positive number.")]
        public decimal Salary { get; set; }

        public string PanCardNumber { get; set; }

        [Required(ErrorMessage = "Employee type required")]
        // [RegularExpression(@"^self-employed|salaried$", ErrorMessage = "Employee type must be self-employed or salaried.")]
        [ValidEmployeeType]
        public string EmployeeType { get; set; }

        [MaxLength(100, ErrorMessage = "Empployer max length is 100 characters.")]
        [ValidEmployer]
        public string Employer { get; set; }
    }
}
