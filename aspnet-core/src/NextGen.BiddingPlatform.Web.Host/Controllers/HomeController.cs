using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace NextGen.BiddingPlatform.Web.Controllers
{
    public class HomeController : BiddingPlatformControllerBase
    {
        [DisableAuditing]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Ui");
        }
    }
}
