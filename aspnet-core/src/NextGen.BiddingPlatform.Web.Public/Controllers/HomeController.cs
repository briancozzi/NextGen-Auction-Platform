using Microsoft.AspNetCore.Mvc;
using NextGen.BiddingPlatform.Web.Controllers;

namespace NextGen.BiddingPlatform.Web.Public.Controllers
{
    public class HomeController : BiddingPlatformControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult GetAuctionItemTemplate()
        {
            return PartialView("~/Views/Home/_AuctionItemTemplate.cshtml");
        }
    }
}