using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.CustomInterface
{
    public interface IHasUniqueIdentifier
    {
        public Guid UniqueId { get; set; }
    }
}
