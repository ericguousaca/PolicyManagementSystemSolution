using System;

namespace PolicyApiLibrary.ViewModels
{
    public class PolicyDetailViewModel : PolicyViewModel
    {
        public int Id { get; set; }

        public string PolicyId { get; set; }

        public decimal MaturityAmount { get; set; }

        public DateTime StartDateInDateTime { get; set; }
    }
}
