using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using NextGen.BiddingPlatform.AuctionHistory.Dto;
using NextGen.BiddingPlatform.Sessions.Dto;
using NextGen.BiddingPlatform.Web.Controllers;
using NextGen.BiddingPlatform.Web.Public.Notification;
using NextGen.BiddingPlatform.Web.Session;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Web.Public.Controllers
{
    [AllowAnonymous]
    public class HomeController : BiddingPlatformControllerBase
    {
        private readonly IPerRequestSessionCache _sessionCache;
        private readonly INotificationManager _notify;
        public HomeController(IPerRequestSessionCache sessionCache, INotificationManager notificationManager)
        {
            _sessionCache = sessionCache;
            _notify = notificationManager;
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
        public ActionResult ProductDetailWithLogin(Guid id, int itemStatus)
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

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Test()
        {
            using (StreamReader reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8))
            {
                var body = await reader.ReadToEndAsync();
                if (body == null)
                {
                    return BadRequest("Unexpected Signature");
                }

                var result = JsonConvert.DeserializeObject<WebHookResponse<GetAuctionBidderHistoryDto>>(body);
                if (result == null)
                    return BadRequest("Error occured while deserializing data");

                await _notify.SendAsync(result.Data.AuctionItemId.ToString(), result.Data);
                //It is certain that Webhook has not been modified.
            }
            return Ok("Success");
        }
    }
}