using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NextGen.BiddingPlatform.Items;

namespace NextGen.BiddingPlatform.Web.Controllers
{
    public class ItemsController : ItemsControllerBase
    {
        public ItemsController(IItemAppService itemService, IWebHostEnvironment environment) : base(itemService, environment)
        {
        }
    }
}