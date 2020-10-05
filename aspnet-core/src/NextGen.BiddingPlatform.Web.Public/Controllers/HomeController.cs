using Microsoft.AspNetCore.Mvc;
using NextGen.BiddingPlatform.Sessions.Dto;
using NextGen.BiddingPlatform.Web.Controllers;
using NextGen.BiddingPlatform.Web.Session;
using System;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Web.Public.Controllers
{
    public class HomeController : BiddingPlatformControllerBase
    {
        private readonly IPerRequestSessionCache _sessionCache;
        public HomeController(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }
        #region pages
        public async Task<ActionResult> Index()
        {
            ViewBag.IsLoggedInUser = await IsCurrentUserLoggedIn();
            return View();
        }
        public ActionResult ProductDetail(Guid id)
        {
            ViewBag.AuctionItemId = id;
            return View();
        }
        public ActionResult ProductDetailWithLogin(Guid id,int itemStatus)
        {
            ViewBag.AuctionItemId = id;
            ViewBag.ItemStatus = itemStatus;
            return View();
        }
        public ActionResult ProductDetailClosed(Guid id, int itemStatus)
        {
            ViewBag.AuctionItemId = id;
            ViewBag.ItemStatus = itemStatus;
            return View();
        }
        public ActionResult ProductDetailClosedWithLogin(Guid id, int itemStatus)
        {
            ViewBag.AuctionItemId = id;
            ViewBag.ItemStatus = itemStatus;
            return View();
        }
        #endregion

        #region Templates
        public IActionResult GetAuctionItemTemplate()
        {
            return PartialView("~/Views/Home/_AuctionItemTemplate.cshtml");
        }
        public IActionResult GetAuctionHistoryTemplate()
        {
            return PartialView("~/Views/Home/_AuctionHistoryTemplate.cshtml");
        }
        #endregion

        private async Task<bool> IsCurrentUserLoggedIn()
        {
            var user = await _sessionCache.GetCurrentLoginInformationsAsync();
            return user.User != null;
        }
    }
}