using CommonLibrary.Messaging.Models;
using System;
using System.Collections.Generic;

namespace CommonLibrary.Messaging.Commands
{
    public interface ISearchPolicyResultCommand
    {
        // public string _id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public DateTime CommandTime { get; set; }
        SearchPolicyCommand SearchPolicyCommand { get; set; }
        List<MessagePolicyDetail> PolicyDetails { get; set; }
    }
}
