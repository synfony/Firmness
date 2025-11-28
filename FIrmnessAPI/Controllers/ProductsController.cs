using AutoMapper;
using Firmness.Core.Data;
using Firmness.Core.Models;
using Firmness.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Firmness.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a list of all products.
        /// </summary>
        /// <returns>A list of products.</returns>
        [HttpGet]
        [AllowAnonymous] // Allow anyone to see the products
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }

        /// <summary>
        /// Gets a specific product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>The requested product.</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return _mapper.Map<ProductDto>(product);
        }

        /// <summary>
        /// Creates a new product. (Admin only)
        /// </summary>
        /// <param name="productDto">The product to create.</param>
        /// <returns>The newly created product.</returns>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ProductDto>> PostProduct(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, _mapper.Map<ProductDto>(product));
        }

        /// <summary>
        /// Updates an existing product. (Admin only)
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="productDto">The updated product data.</param>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutProduct(int id, ProductDto productDto)
        {
            if (id != productDto.Id)
            {
                return BadRequest();
            }

            var product = _mapper.Map<Product>(productDto);
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a product. (Admin only)
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
