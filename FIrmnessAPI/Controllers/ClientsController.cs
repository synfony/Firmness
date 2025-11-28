using AutoMapper;
using Firmness.Core.Data;
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
    [Authorize(Roles = "Administrator")]
    public class ClientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ClientsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Clients
        /// <summary>
        /// Retrieves a list of all clients. (Admin only)
        /// </summary>
        /// <returns>A list of ClientDto objects.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetClients()
        {
            // Ensure User is included for mapping Email
            var clients = await _context.Clients.Include(c => c.User).ToListAsync();
            return _mapper.Map<List<ClientDto>>(clients);
        }

        // GET: api/Clients/5
        /// <summary>
        /// Retrieves a specific client by ID. (Admin only)
        /// </summary>
        /// <param name="id">The ID of the client.</param>
        /// <returns>A ClientDto object.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDto>> GetClient(int id)
        {
            // Ensure User is included for mapping Email
            var client = await _context.Clients.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            return _mapper.Map<ClientDto>(client);
        }
    }
}
