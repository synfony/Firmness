using Firmness.Core.Data;
using Firmness.Core.Models;
using Firmness.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace Firmness.Web.Controllers.Admin
{
    [Authorize(Roles = "Administrator")]
    [Route("Admin/[controller]/[action]/{id?}")]
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PdfService _pdfService;
        private readonly IWebHostEnvironment _env;

        public SalesController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _pdfService = new PdfService();
            _env = env;
        }

        // GET: Sales
        public async Task<IActionResult> Index()
        {
            var sales = await _context.Sales.Include(s => s.Client).ToListAsync();
            return View("~/Views/Admin/Sales/Index.cshtml", sales);
        }

        // GET: Sales/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Clients = await _context.Clients.ToListAsync();
            ViewBag.Products = await _context.Products.ToListAsync();
            return View("~/Views/Admin/Sales/Create.cshtml");
        }

        // POST: Sales/Create
        [HttpPost]
        public async Task<IActionResult> Create(Sale model)
        {
            model.SaleDate = DateTime.UtcNow;

            if (model.SaleDetails == null || !model.SaleDetails.Any())
            {
                ModelState.AddModelError("", "Debe agregar al menos un producto a la venta.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Clients = await _context.Clients.ToListAsync();
                ViewBag.Products = await _context.Products.ToListAsync();
                return View("~/Views/Admin/Sales/Create.cshtml", model);
            }

            model.Client = await _context.Clients.FindAsync(model.ClientId);
            if (model.Client == null)
            {
                ModelState.AddModelError("", "El cliente seleccionado no es válido.");
                ViewBag.Clients = await _context.Clients.ToListAsync();
                ViewBag.Products = await _context.Products.ToListAsync();
                return View("~/Views/Admin/Sales/Create.cshtml", model);
            }

            foreach (var detail in model.SaleDetails)
            {
                var product = await _context.Products.FindAsync(detail.ProductId);
                if (product != null)
                {
                    detail.Product = product;
                    detail.UnitPrice = product.Price;
                }
            }
            
            _context.Sales.Add(model);
            await _context.SaveChangesAsync();

            var pdfBytes = _pdfService.GenerateReceipt(model);
            string wwwRootPath = _env.WebRootPath;
            string fileName = $"recibo-{model.Id}-{DateTime.Now:yyyyMMddHHmmss}.pdf";
            string path = Path.Combine(wwwRootPath, "recibos", fileName);

            var directory = Path.GetDirectoryName(path);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await System.IO.File.WriteAllBytesAsync(path, pdfBytes);
            model.ReceiptUrl = $"/recibos/{fileName}";

            _context.Sales.Update(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Sales/RegeneratePdfs
        [HttpPost]
        public async Task<IActionResult> RegeneratePdfs()
        {
            var sales = await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .ToListAsync();

            string wwwRootPath = _env.WebRootPath;
            var recibosDir = Path.Combine(wwwRootPath, "recibos");

            foreach (var sale in sales)
            {
                var pdfBytes = _pdfService.GenerateReceipt(sale);
                string fileName = $"recibo-{sale.Id}-{DateTime.Now:yyyyMMddHHmmss}.pdf";
                string path = Path.Combine(recibosDir, fileName);

                await System.IO.File.WriteAllBytesAsync(path, pdfBytes);
                sale.ReceiptUrl = $"/recibos/{fileName}";
                _context.Sales.Update(sale);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
