using Firmness.Web.Data;
using Firmness.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Firmness.Web.Controllers.Admin
{
    [Authorize(Roles = "Administrator")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View("~/Views/Admin/Products/Index.cshtml", products);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View("~/Views/Admin/Products/Create.cshtml");
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/Products/Create.cshtml", model);

            if (ImageFile != null)
            {
                string wwwRootPath = _env.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                string path = Path.Combine(wwwRootPath, "images/Products", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                model.ImageUrl = "/images/Products/" + fileName;
            }

            _context.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            return View("~/Views/Admin/Products/Edit.cshtml", product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product model, IFormFile? ImageFile)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View("~/Views/Admin/Products/Edit.cshtml", model);

            var dbProduct = await _context.Products.FindAsync(id);

            if (dbProduct == null)
                return NotFound();

            dbProduct.Name = model.Name;
            dbProduct.Description = model.Description;
            dbProduct.Price = model.Price;
            dbProduct.Stock = model.Stock;

            if (ImageFile != null)
            {
                string wwwRootPath = _env.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                string path = Path.Combine(wwwRootPath, "images/Products", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(dbProduct.ImageUrl))
                {
                    string oldImagePath = Path.Combine(wwwRootPath, dbProduct.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                dbProduct.ImageUrl = "/images/Products/" + fileName;
            }

            _context.Update(dbProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            return View("~/Views/Admin/Products/Delete.cshtml", product);
        }

        // POST: Products/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                string path = Path.Combine(_env.WebRootPath, product.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
