using NextGen.BiddingPlatform.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.Items.Dto
{
    public class Dropdowns
    {
        public Dropdowns()
        {
            ProcurementStates = new List<ListDto>();
            ItemStatus = new List<ListDto>();
            Visibilities = new List<ListDto>();
        }
        public List<ListDto> ProcurementStates { get; set; }
        public List<ListDto> ItemStatus { get; set; }
        public List<ListDto> Visibilities { get; set; }
    }
}
