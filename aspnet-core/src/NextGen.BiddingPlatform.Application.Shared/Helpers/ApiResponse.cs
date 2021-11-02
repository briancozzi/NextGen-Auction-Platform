using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NextGen.BiddingPlatform.Helpers
{
    public class ApiResponse<T> where T : class, new()
    {
        public T Data { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
