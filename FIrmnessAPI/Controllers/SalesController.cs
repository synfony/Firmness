using AutoMapper;
using Firmness.Core.Data;
using Firmness.Core.Models;
using Firmness.Core.Services;
using Firmness.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Firmness.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SalesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public SalesController(ApplicationDbContext context, IMapper mapper, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
        }

        // POST: api/Sales
        [HttpPost]
        public async Task<ActionResult<SaleDto>> PostSale(SaleDto saleDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            // Include the related User to get the email address
            var client = await _context.Clients.Include(c => c.User).FirstOrDefaultAsync(c => c.UserId == userId);
            if (client == null || client.User == null)
            {
                return NotFound("Client or associated user not found for the authenticated user.");
            }

            var sale = new Sale
            {
                ClientId = client.Id,
                SaleDate = DateTime.UtcNow,
                SaleDetails = saleDto.SaleDetails.Select(sd => new SaleDetail
                {
                    ProductId = sd.ProductId,
                    Quantity = sd.Quantity,
                    UnitPrice = sd.UnitPrice
                }).ToList()
            };

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            // Send confirmation email
            var subject = "Your Firmness Order Confirmation";
            var message = $"<h1>Thank you for your purchase, {client.FirstName}!</h1><p>Your order with a total of ${sale.SaleDetails.Sum(sd => sd.Quantity * sd.UnitPrice)} has been processed.</p>";
            await _emailService.SendEmailAsync(client.User.Email!, subject, message);

            var createdSaleDto = _mapper.Map<SaleDto>(sale);
            return CreatedAtAction(nameof(GetSale), new { id = sale.Id }, createdSaleDto);
        }

        // GET: api/Sales/5 (Required for CreatedAtAction)
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleDto>> GetSale(int id)
        {
            var sale = await _context.Sales
                .Include(s => s.SaleDetails)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null)
            {
                return NotFound();
            }

            var saleDto = _mapper.Map<SaleDto>(sale);
            return saleDto;
        }
    }
}