using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Web.Public.Notification
{
    public class WebHookResponse<T>
    {
        public string Id { get; set; }
        public string Event { get; set; }
        public string Attempt { get; set; }
        public T Data { get; set; }
        public string CreationTimeUtc { get; set; }
    }
}
