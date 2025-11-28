using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Firmness.Web.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using Firmness.Core.Models;

namespace Firmness.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            ILogger<HomeController> logger, 
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    ViewData["UserRoles"] = roles;
                }
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel 
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            });
        }
    }
}
