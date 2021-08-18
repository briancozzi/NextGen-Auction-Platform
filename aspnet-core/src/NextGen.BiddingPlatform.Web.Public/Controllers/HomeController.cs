using Abp.Runtime.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using NextGen.BiddingPlatform.AuctionHistory.Dto;
using NextGen.BiddingPlatform.Sessions.Dto;
using NextGen.BiddingPlatform.Web.Controllers;
using NextGen.BiddingPlatform.Web.Public.Notification;
using NextGen.BiddingPlatform.Web.Session;
using NextGen.BiddingPlatform.WebHooks;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Web.Public.Controllers
{
    [AllowAnonymous]
    public class HomeController : BiddingPlatformControllerBase
    {
        private readonly IPerRequestSessionCache _sessionCache;
        private readonly INotificationManager _notify;
        private readonly IWebhookSubscriptionAppService _webhookSubscriptionService;
        public HomeController(IPerRequestSessionCache sessionCache,
                                                  INotificationManager notificationManager,
                                                  IWebhookSubscriptionAppService webhookSubscriptionService)
        {
            _sessionCache = sessionCache;
            _notify = notificationManager;
            _webhookSubscriptionService = webhookSubscriptionService;
        }
        #region pages

        public async Task<IActionResult> CurrentBids()
        {
            return View();
        }
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
        public async Task<ActionResult> ProductDetailWithLogin(Guid id, int itemStatus)
        {
            ViewBag.AuctionItemId = id;
            ViewBag.ItemStatus = itemStatus;
            var user = await _sessionCache.GetCurrentLoginInformationsAsync();
            ViewBag.TenantId = user?.Tenant?.Id;
            ViewBag.UserId = user?.User?.Id;
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

        private async Task<bool> IsCurrentUserLoggedIn()
        {
            var user = await _sessionCache.GetCurrentLoginInformationsAsync();
            return user.User != null;
        }

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> WebhookReceiver()
        {
            try
            {
                using (StreamReader reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8))
                {
                    var body = await reader.ReadToEndAsync();
                    if (body == null)
                        return BadRequest("Data not found");

                    var result = JsonConvert.DeserializeObject<WebHookResponse<GetAuctionBidderHistoryDto>>(body);
                    if (result == null)
                        return BadRequest("Error occured while deserializing data");

                    //right now we don't need below code because we are able to sent webhooks to specific tenant during publish webhook

                    if (!await _webhookSubscriptionService.IsWebhookSubscribed(result.Data?.TenantId, result.Event))
                        return BadRequest("Webhook not subscribe by this user");

                    if (!await IsSignatureValid(result.Event, body, result.Data?.TenantId))
                        return BadRequest("Unexpected signature");

                    await _notify.SendAsync(result.Data.AuctionItemId.ToString(), result.Data);
                }
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error occured");
            }
        }

        private async Task<bool> IsSignatureValid(string webhookName, string body, int? tenantId)
        {
            //we also can gets the subscriptions based on tenantId
            var userSubscriptions = await _webhookSubscriptionService.GetTenantsAllSubscriptions(tenantId);
            var webhookSecrets = userSubscriptions.Items.Where(x => x.Webhooks.Contains(webhookName)).Select(x => x.Secret);
            bool isValidSignature = false;
            foreach (var secret in webhookSecrets)
            {
                isValidSignature = IsSignatureCompatible(secret, body);
                if (isValidSignature)
                    break;
            }
            return isValidSignature;
        }

        private bool IsSignatureCompatible(string secret, string body)
        {
            if (!HttpContext.Request.Headers.ContainsKey("abp-webhook-signature"))
            {
                return false;
            }

            var receivedSignature = HttpContext.Request.Headers["abp-webhook-signature"].ToString().Split("=");//will be something like "sha256=whs_XXXXXXXXXXXXXX"
                                                                                                               //It starts with hash method name (currently "sha256") then continue with signature. You can also check if your hash method is true.

            string computedSignature;
            switch (receivedSignature[0])
            {
                case "sha256":
                    var secretBytes = Encoding.UTF8.GetBytes(secret);
                    using (var hasher = new HMACSHA256(secretBytes))
                    {
                        var data = Encoding.UTF8.GetBytes(body);
                        computedSignature = BitConverter.ToString(hasher.ComputeHash(data));
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            return computedSignature == receivedSignature[1];
        }
    }
}