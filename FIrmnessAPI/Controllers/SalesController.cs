using AutoMapper;
using Firmness.Core.Data;
using Firmness.Core.Models;
using Firmness.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firmness.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SalesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Sales
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<SaleDto>>> GetSales()
        {
            var sales = await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .ToListAsync();
            return _mapper.Map<List<SaleDto>>(sales);
        }

        // POST: api/Sales
        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<ActionResult<SaleDto>> PostSale(SaleDto saleDto)
        {
            var sale = _mapper.Map<Sale>(saleDto);
            sale.SaleDate = DateTime.UtcNow;

            // You might want to get the ClientId from the authenticated user's claims
            // For now, we'll trust the DTO
            
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            var createdSale = await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .FirstOrDefaultAsync(s => s.Id == sale.Id);

            return CreatedAtAction(nameof(GetSales), new { id = sale.Id }, _mapper.Map<SaleDto>(createdSale));
        }
    }
}
