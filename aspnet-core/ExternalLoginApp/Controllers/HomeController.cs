using ExternalLoginApp.Data;
using ExternalLoginApp.Data.DataModel;
using ExternalLoginApp.Helper;
using ExternalLoginApp.Models;
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
        public HomeController(ILogger<HomeController> logger,
                              IConfiguration configuration,
                              UserManager<IdentityUser> userManager,
                              ApplicationDbContext context)
        {
            _logger = logger;
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
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
            ResponseVM<object> response = await DataHelper<object>.Execute(_configuration["Bidding:ApiUrl"], route, OperationType.GET);

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
    }
}
