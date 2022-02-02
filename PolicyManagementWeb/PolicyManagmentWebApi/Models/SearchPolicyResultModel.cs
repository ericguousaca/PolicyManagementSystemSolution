using System.Collections.Generic;

namespace PolicyManagementWebApi.Models
{
    public class SearchPolicyResultModel
    {
        public int Id { get; set; }
        public string PolicyId { get; set; }
        public string PolicyName { get; set; }
        public string StartDate { get; set; }
        public int DurationInYears { get; set; }
        public string CompanyName { get; set; }
        public decimal InitialDeposit { get; set; }
        public string PolicyType { get; set; }
        public List<string> UserTypes { get; set; }
        public int TermsPerYear { get; set; }
        public decimal TermAmount { get; set; }
        public double Interest { get; set; }
        public decimal MaturityAmount { get; set; }
    }
}
