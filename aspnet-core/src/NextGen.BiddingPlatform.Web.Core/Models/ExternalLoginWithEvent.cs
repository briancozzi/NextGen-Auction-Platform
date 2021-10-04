using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Web.Models
{
    public class ExternalLoginWithEvent
    {
        public Guid UserUniqueId { get; set; }
        public string UserId { get; set; }
        public int EventId { get; set; }
        public int? TenantId { get; set; }
    }
}
