using System;

namespace CommonLibrary
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string PublicMessage { get; set; }
        public string InternalMessge { get; set; }

        public Exception Exception { get; set; }

        public BaseResponse()
        {
            this.Success = false;
            this.Exception = null;
        }

    }
}
