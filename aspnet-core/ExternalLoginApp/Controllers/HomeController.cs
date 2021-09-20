using ExternalLoginApp.Helper;
using ExternalLoginApp.Models;
using Microsoft.AspNetCore.Mvc;
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
        public HomeController(ILogger<HomeController> logger,
                              IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GoToBidApp()
        {
            ResponseVM<object> response = await DataHelper<object>.Execute(_configuration["Bidding:ApiUrl"], "/api/TokenAuth/ExternalEventlifyLogin", OperationType.POST, new
            {
                FirstName = "Test",
                LastName = "User",
                EmailAddress = User.Identity.Name,
                TenantId = "1"
            });

            return Redirect(response.Result.Data.ToString());
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
