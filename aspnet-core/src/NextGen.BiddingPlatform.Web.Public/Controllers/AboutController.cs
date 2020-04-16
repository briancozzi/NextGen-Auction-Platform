using Microsoft.AspNetCore.Mvc;
using NextGen.BiddingPlatform.Web.Controllers;

namespace NextGen.BiddingPlatform.Web.Public.Controllers
{
    public class AboutController : BiddingPlatformControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}