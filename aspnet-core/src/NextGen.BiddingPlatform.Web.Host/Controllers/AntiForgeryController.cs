using Microsoft.AspNetCore.Antiforgery;

namespace NextGen.BiddingPlatform.Web.Controllers
{
    public class AntiForgeryController : BiddingPlatformControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
