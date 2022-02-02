using System;

namespace CommonLibrary.Messaging.Commands
{
    public class SearchPolicyCommand : ISearchPolicyCommand
    {
        public string SearchId { get; set; }
        public string PolicyType { get; set; }
        public int NumberOfYears { get; set; }
        public string CompanyName { get; set; }
        public string PolicyId { get; set; }
        public string PolicyName { get; set; }
        public DateTime CommandTime { get; set; }

        public SearchPolicyCommand()
        {
            this.SearchId = "";
            this.PolicyType = "";
            this.NumberOfYears = 0;
            this.CompanyName = "";
            this.PolicyId = "";
            this.PolicyName = "";
            this.CommandTime = DateTime.MinValue;
        }
    }
}
