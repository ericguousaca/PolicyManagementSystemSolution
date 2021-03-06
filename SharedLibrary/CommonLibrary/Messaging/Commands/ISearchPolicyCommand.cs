using System;

namespace CommonLibrary.Messaging.Commands
{
    public interface ISearchPolicyCommand
    {
        public string SearchId { get; set; }
        public string PolicyType { get; set; }
        public int NumberOfYears { get; set; }
        public string CompanyName { get; set; }
        public string PolicyId { get; set; }
        public string PolicyName { get; set; }
        public DateTime CommandTime { get; set; }
    }
}
