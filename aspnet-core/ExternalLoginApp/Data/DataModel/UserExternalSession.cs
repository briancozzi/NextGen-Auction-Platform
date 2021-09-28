using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExternalLoginApp.Data.DataModel
{
    [Table("UserExternalSessions")]
    public class UserExternalSession
    {
        [Key]
        public Guid UniqueId { get; set; }
        public string UserId { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
