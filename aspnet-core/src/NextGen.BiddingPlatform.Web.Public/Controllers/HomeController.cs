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
        public ActionResult ProductDetail()
        {
            return View();
        }
        public ActionResult ProductDetailClosed()
        {
            return View();
        }
        public ActionResult ProductDetailWithLogin()
        {
            return View();
        }
        public ActionResult ProductDetailClosedWithLogin()
        {
            return View();
        }
    }
}