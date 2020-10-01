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