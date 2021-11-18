using Abp.Authorization;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using NextGen.BiddingPlatform.AppAccountEvent.Dto;
using NextGen.BiddingPlatform.ApplicationConfigurations;
using NextGen.BiddingPlatform.AuctionHistory;
using NextGen.BiddingPlatform.AuctionHistory.Dto;
using NextGen.BiddingPlatform.AuctionItem;
using NextGen.BiddingPlatform.AuctionItem.Dto;
using NextGen.BiddingPlatform.Sessions.Dto;
using NextGen.BiddingPlatform.UserEvents;
using NextGen.BiddingPlatform.Web.Controllers;
using NextGen.BiddingPlatform.Web.Public.Notification;
using NextGen.BiddingPlatform.Web.Session;
using NextGen.BiddingPlatform.WebHooks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        private readonly IAuctionItemAppService _auctionItemAppService;
        private readonly IAuctionHistoryAppService _auctionHistoryAppService;
        private readonly IApplicationConfigurationsAppService _appConfigAppService;
        private readonly IUserEventsAppService _userEventsAppService;
        public HomeController(IPerRequestSessionCache sessionCache,
                              INotificationManager notificationManager,
                              IWebhookSubscriptionAppService webhookSubscriptionService,
                              IAuctionItemAppService auctionItemAppService,
                              IAuctionHistoryAppService auctionHistoryAppService,
                              IApplicationConfigurationsAppService appConfigAppService,
                              IUserEventsAppService userEventsAppService)
        {
            _sessionCache = sessionCache;
            _notify = notificationManager;
            _webhookSubscriptionService = webhookSubscriptionService;
            _auctionItemAppService = auctionItemAppService;
            _auctionHistoryAppService = auctionHistoryAppService;
            _appConfigAppService = appConfigAppService;
            _userEventsAppService = userEventsAppService;
        }
        #region pages

        public async Task<IActionResult> CurrentBids()
        {
            var user = await _sessionCache.GetCurrentLoginInformationsAsync();
            ViewBag.UserId = user?.User?.Id;
            return View();
        }
        public async Task<IActionResult> WatchList()
        {
            var user = await _sessionCache.GetCurrentLoginInformationsAsync();
            ViewBag.UserId = user?.User?.Id;
            ViewBag.TenantId = user?.Tenant?.Id;
            return View();
        }

        public async Task<IActionResult> RecentlyViewed()
        {
            var user = await _sessionCache.GetCurrentLoginInformationsAsync();
            ViewBag.UserId = user?.User?.Id;
            ViewBag.TenantId = user?.Tenant?.Id;
            return View();
        }

        public async Task<ActionResult> Index(Guid eventId)
        {
            ViewBag.IsLoggedInUser = await IsCurrentUserLoggedIn();
            ViewBag.EventId = eventId;
            return View();
        }
        public ActionResult ProductDetail(Guid id)
        {
            ViewBag.AuctionItemId = id;
            return View();
        }
        public ActionResult ProductDetailWithLogin(Guid id, int itemStatus, Guid eventId)
        {
            ViewBag.AuctionItemId = id;
            ViewBag.ItemStatus = itemStatus;
            ViewBag.EventId = eventId;
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

        public IActionResult PublicEvents()
        {
            return View();
        }

        public IActionResult PublicEventItems(Guid eventId)
        {
            ViewBag.EventId = eventId;
            return View();
        }

        public IActionResult PublicEventAuctionItem(Guid eventId, Guid auctionItemId)
        {
            ViewBag.AuctionItemId = auctionItemId;
            ViewBag.EventId = eventId;
            return View();
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
                    await _notify.UpdateCurrentBidsAsync();
                }
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error occured");
            }
        }

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> CloseEventItemWebhookReceiver()
        {
            try
            {
                using (StreamReader reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8))
                {
                    var body = await reader.ReadToEndAsync();
                    if (body == null)
                        return BadRequest("Data not found");

                    var result = JsonConvert.DeserializeObject<WebHookResponse<CloseEventOrItemDto>>(body);
                    if (result == null)
                        return BadRequest("Error occured while deserializing data");

                    //right now we don't need below code because we are able to sent webhooks to specific tenant during publish webhook

                    if (!await _webhookSubscriptionService.IsWebhookSubscribed(result.Data?.TenantId, result.Event))
                        return BadRequest("Webhook not subscribe by this user");

                    if (!await IsSignatureValid(result.Event, body, result.Data?.TenantId))
                        return BadRequest("Unexpected signature");

                    await _notify.CloseBiddingForEventOrItem(result.Data);
                    //await _notify.UpdateCurrentBidsAsync();
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

        //send data to eventlify means external app

        [HttpGet]
        [AbpAuthorize]
        public async Task<IActionResult> SendDataToExternalApp(Guid eventId)
        {
            try
            {
                var payload = await _auctionItemAppService.GetEventWinners(eventId);

                var route = await _appConfigAppService.GetConfigByKey("WinnerApiResponse");

                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Clear();

                var data = JsonConvert.SerializeObject(payload.Data);
                var stringContent = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponse = await _client.PostAsync(route, stringContent);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "Error occured while sending data to external app!!"
                    });
                }
                else
                {
                    return Json(new
                    {
                        Success = true,
                        Message = "Data sent successfully."
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}