using System;
using System.Collections.Generic;
using System.Text;

namespace NextGen.BiddingPlatform.CustomAuthorization
{
    public class CustomEnum
    {
        public enum ServiceType
        {
            AppAccount,
            AccountEvent
        }
        public enum Permission
        {
            Full, Create, Edit, Delete
        }

        public enum AccessType
        {
            List, Get, Delete, Edit
        }
    }
}
