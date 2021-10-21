using ExternalLoginApp.Data;
using ExternalLoginApp.Data.DataModel;
using ExternalLoginApp.Helper;
using ExternalLoginApp.Models;
using ExternalLoginApp.Models.ApiModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ExternalLoginApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(ILogger<HomeController> logger,
                              IConfiguration configuration,
                              UserManager<IdentityUser> userManager,
                              ApplicationDbContext context,
                              IHttpContextAccessor httpAccessor,
                              SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
            _httpAccessor = httpAccessor;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetEvents()
        {
            var route = "/api/services/app/AccountEvent/GetAllAnnonymousAccountEvents";
            ResponseVM<AuctionEvents> response = await DataHelper<AuctionEvents>.Execute(
                _configuration["Bidding:ApiUrl"],
                route,
                _configuration["Bidding:TenantId"], OperationType.GET);

            var result = new List<AccountEventListDto>();
            if (response.Result == null || response.Result.Data == null) { }

            result = response.Result.Data.Items;

            return PartialView("~/Views/Home/_AllEventsPartialView.cshtml", result);
        }

        //register to event
        public async Task<IActionResult> RegisterToEvent(int eventId)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
                return RedirectToAction("Index");

            var tenantId = _configuration["Bidding:TenantId"];

            var activeUserSession = new UserExternalSession
            {
                UserId = user.Id,
                ExpireAt = DateTime.Now.AddMinutes(5)
            };
            await _context.UserExternalSessions.AddAsync(activeUserSession);
            await _context.SaveChangesAsync();

            var route = $"/api/TokenAuth/ExternalUserLoginWithEvent";

            var input = new
            {
                userUniqueId = activeUserSession.UniqueId,
                userId = user.Id,
                eventId = eventId,
                tenantId = tenantId
            };

            ResponseVM<AuthenticateResultModel> response = await DataHelper<AuthenticateResultModel>.Execute(
                _configuration["Bidding:ApiUrl"],
                route,
                tenantId,
                OperationType.POST,
                input
                );

            if (response.Result == null)
                return Json(new { Success = false, Message = "Internal Server Error !!" });
            else if (!response.Result.Success)
                return Json(new { Success = false, Message = response.Result.Error.message });
            else
            {
                var data = response.Result.Data;
                _httpAccessor.HttpContext.Response.Cookies.Append("AuthToken", data.AccessToken, new CookieOptions
                {
                    Expires = DateTime.Now.AddSeconds(data.ExpireInSeconds)
                });
                data.ReturnUrl = data.ReturnUrl + $"&eventId={eventId}";
                _httpAccessor.HttpContext.Response.Cookies.Append("BiddingReturnUrl", data.ReturnUrl, new CookieOptions
                {
                    Expires = DateTime.Now.AddSeconds(data.ExpireInSeconds)
                });
                return Json(new { Success = true, Message = "" });
            }

        }

        //get event details
        public async Task<IActionResult> Event(int eventId)
        {
            var route = "/api/services/app/AccountEvent/GetEventById?id=" + eventId;

            ResponseVM<EventWithAuctionItems> response = await DataHelper<EventWithAuctionItems>.ExecuteWithToken(
                _configuration["Bidding:ApiUrl"],
                route,
                _configuration["Bidding:TenantId"].ToString(),
                OperationType.GET,
                 _httpAccessor.HttpContext.Request.Cookies["AuthToken"].ToString()
                );


            return View(response.Result.Data);
        }

        //create bidder and start bidding
        public async Task<IActionResult> CreateBidderAndPlaceABid(int eventId)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var input = new
            {
                externalUserId = user.Id,
                eventId = eventId
            };

            var route = $"/api/services/app/AuctionBidder/CreateBidderFromExternalApp";
            ResponseVM<object> response = await DataHelper<object>.ExecuteWithToken(
                _configuration["Bidding:ApiUrl"],
                route,
                _configuration["Bidding:TenantId"],
                OperationType.POST,
                _httpAccessor.HttpContext.Request.Cookies["AuthToken"].ToString(),
                input
                );

            if (response.Result == null)
                return Json(new { Success = false, Message = "Internal Server Error !!" });
            else if (!response.Result.Success)
                return Json(new { Success = false, Message = response.Result.Error.message });
            else
            {
                return Json(new { Success = true, Message = "" });
            }
        }

        public IActionResult GoToBiddingApp()
        {
            return Redirect(_httpAccessor.HttpContext.Request.Cookies["BiddingReturnUrl"].ToString());
        }

        public IActionResult GoToAuctionItems(Guid auctionItemId, int itemStatus)
        {
            var productUrl = $"https://localhost:44303/Home/ProductDetailWithLogin?id={auctionItemId}&itemStatus={itemStatus}";
            var returlUrl = _httpAccessor.HttpContext.Request.Cookies["BiddingReturnUrl"].ToString() + "&returnUrl=" + productUrl;
            return Redirect(returlUrl);
        }

        public async Task<IActionResult> GoToBidApp()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var activeUserSession = new UserExternalSession
            {
                UserId = user.Id,
                ExpireAt = DateTime.Now.AddMinutes(5)
            };
            await _context.UserExternalSessions.AddAsync(activeUserSession);
            await _context.SaveChangesAsync();

            var route = $"/api/TokenAuth/ExternalUserLogin?UniqueId={activeUserSession.UniqueId}&UserId={activeUserSession.UserId}&TenantId=1";
            ResponseVM<object> response = await DataHelper<object>.Execute(
                _configuration["Bidding:ApiUrl"],
                route,
                _configuration["Bidding:TenantId"], OperationType.GET);

            if (response.Result == null || response.Result.Data == null)
                return RedirectToAction("Error", "Home");
            else
                return Redirect(response.Result.Data.ToString());
        }

        public async Task<IActionResult> GetUserDetails(Guid uniqueId, string userId, int tenantId)
        {
            var userActiveSession = await _context.UserExternalSessions.FirstOrDefaultAsync(s => s.UniqueId == uniqueId && s.UserId == userId);

            if (DateTime.Now >= userActiveSession.ExpireAt)
            {
                //throw exception
                throw new Exception("User link already expired!!");
            }
            else
            {
                var user = await _userManager.FindByIdAsync(userActiveSession.UserId);
                var userDetails = new
                {
                    FirstName = "Test",
                    LastName = "User",
                    TenantId = tenantId,
                    EmailAddress = user.Email,
                    ExternalUserId = userActiveSession.UserId,
                };

                _context.UserExternalSessions.Remove(userActiveSession);
                //send user details
                return Json(userDetails);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
