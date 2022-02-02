using PolicyApiLibrary.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PolicyApiLibrary.ViewModels
{
    public class PolicyViewModel
    {


        [Required]
        [MaxLength(100, ErrorMessage = "Prolicy name max length is 100 charaters.")]
        public string PolicyName { get; set; }

        [Required]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$", ErrorMessage = "Start date shall be the format of dd/mm/yyyy.")]
        [ValidStartDateString]
        public string StartDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Duration years must be positive integer.")]
        public int DurationInYears { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Company name max length is 100 charaters.")]
        public string CompanyName { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Initial deposit cannot be negative value.")]
        public decimal InitialDeposit { get; set; }

        [Required(ErrorMessage = "Policy type is required")]
        [ValidPolicyType]
        public string PolicyType { get; set; }

        [Required(ErrorMessage = "User types are required, can accept multiple User Types separated by ',', like \"A,B,C\".")]
        [ValidUserTypes]
        public List<string> UserTypes { get; set; }

        [Range(1, 12, ErrorMessage = "The minimum term per year is 1 and max is 12.")]
        public int TermsPerYear { get; set; }

        [ValidPositiveDecimal(ErrorMessage = "Term amount must be positive value.")]
        public decimal TermAmount { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "Interest value must positive number")]
        public double Interest { get; set; }


    }
}
