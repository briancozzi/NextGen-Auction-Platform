using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Abp.Domain.Entities.Auditing;

namespace NextGen.Auction.Address
{
    [Table("States")]
    public class State : AuditedEntity<Guid>
    {
        public string StateName { get; set; }
        public string StateCode { get; set; }
    }
}
