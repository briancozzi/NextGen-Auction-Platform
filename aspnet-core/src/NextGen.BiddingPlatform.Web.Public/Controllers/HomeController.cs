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
    }
}