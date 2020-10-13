using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NextGen.BiddingPlatform.Enums
{
    public class ItemEnums
    {
        public enum ProcurementState
        {
            [Display(Name = "In Hand")]
            InHand,
            [Display(Name = "Shipped On Address")]
            ShippedOnAddress
        }

        public enum ItemStatus
        {
            [Display(Name = "Open")]
            Open,
            [Display(Name = "Closed")]
            Closed
        }
        public enum Visibility
        {
            [Display(Name = "Live")]
            Live
        }

        public enum BiddingStatus
        {
            [Display(Name = "Pending")]
            Pending,
            [Display(Name = "Winning")]
            Winning
        }
    }
}
