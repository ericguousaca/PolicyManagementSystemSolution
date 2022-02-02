using CommonLibrary.Messaging.Models;
using System;
using System.Collections.Generic;

namespace CommonLibrary.Messaging.Commands
{
    public class SearchPolicyResultCommand : ISearchPolicyResultCommand
    {
        // _id is needed for saving to MongoDb and SerializeObject and DeserializeObject etc.        
        public string _id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public DateTime CommandTime { get; set; }
        public SearchPolicyCommand SearchPolicyCommand { get; set; }
        public List<MessagePolicyDetail> PolicyDetails { get; set; }

        public SearchPolicyResultCommand()
        {
            this._id = Guid.NewGuid().ToString();
            this.Success = false;
            this.Message = "";
            this.CommandTime = DateTime.MinValue;
            this.SearchPolicyCommand = new SearchPolicyCommand();
            this.PolicyDetails = new List<MessagePolicyDetail> { };
        }
    }
}
