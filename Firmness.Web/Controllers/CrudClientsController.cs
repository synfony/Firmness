using Firmness.Core.Data;
using Firmness.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firmness.Web.Controllers.Admin
{
    [Authorize(Roles = "Administrator")]
    [Route("Admin/[controller]/[action]/{id?}")]
    public class CrudClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CrudClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================
        // LISTAR CLIENTES (READ)
        // ============================
        public async Task<IActionResult> Index()
        {
            var clients = await _context.Clients.ToListAsync();
            return View("~/Views/Admin/Clients/Index.cshtml", clients);
        }

        // ============================
        // EDITAR CLIENTE (GET)
        // ============================
        public async Task<IActionResult> Edit(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
                return NotFound();

            return View("~/Views/Admin/Clients/Edit.cshtml", client);
        }

        // ============================
        // EDITAR CLIENTE (POST)
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.Id)
                return BadRequest();

            var dbClient = await _context.Clients.FindAsync(id);
            if (dbClient == null)
            {
                return NotFound();
            }

            client.PersonType = dbClient.PersonType;

            ModelState.Clear();
            TryValidateModel(client);

            if (!ModelState.IsValid)
                return View("~/Views/Admin/Clients/Edit.cshtml", client);

            try
            {
                dbClient.FirstName = client.FirstName;
                dbClient.LastName = client.LastName;
                dbClient.DocumentId = client.DocumentId;
                dbClient.Address = client.Address;
                dbClient.PhoneNumber = client.PhoneNumber;
                dbClient.PurchaseHistory = client.PurchaseHistory;

                _context.Update(dbClient);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Clients.Any(e => e.Id == client.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // ============================
        // ELIMINAR CLIENTE (GET)
        // ============================
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
                return NotFound();

            return View("~/Views/Admin/Clients/Delete.cshtml", client);
        }

        // ============================
        // ELIMINAR CLIENTE (POST)
        // ============================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
                return NotFound();

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
