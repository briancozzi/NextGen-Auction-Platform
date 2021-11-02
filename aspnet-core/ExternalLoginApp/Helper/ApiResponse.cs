using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ExternalLoginApp.Helper
{
    public class ApiResponse<T> where T: class, new()
    {
        public T Data { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
