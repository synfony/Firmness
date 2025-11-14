using Firmness.Web.Data;
using Firmness.Web.Models.ModelsView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Firmness.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalProducts = await _context.Products.CountAsync(),
                TotalClients = await _context.Clients.CountAsync(),
                TotalSales = await _context.Sales.CountAsync()
            };

            return View(viewModel);
        }
    }
}
