using ConstructionSaaSBackend.Data;
using ConstructionSaaSBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionSaaSBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            var tenantId = GetCurrentTenantId();
            var clients = await _context.Clients
                .Include(c => c.Tenant)
                .Where(c => c.TenantId == tenantId)
                .ToListAsync();
            
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient(int id)
        {
            var tenantId = GetCurrentTenantId();
            var client = await _context.Clients
                .Include(c => c.Tenant)
                .FirstOrDefaultAsync(c => c.ClientId == id && c.TenantId == tenantId);
            
            if (client == null)
            {
                return NotFound();
            }
            
            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Set tenant ID from current context
            client.TenantId = GetCurrentTenantId();

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetClient), new { id = client.ClientId }, client);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenantId = GetCurrentTenantId();
            var existingClient = await _context.Clients
                .FirstOrDefaultAsync(c => c.ClientId == id && c.TenantId == tenantId);
            
            if (existingClient == null)
            {
                return NotFound();
            }

            // Update properties
            existingClient.Name = client.Name;
            existingClient.ContactPerson = client.ContactPerson;
            existingClient.Email = client.Email;
            existingClient.PhoneNumber = client.PhoneNumber;
            existingClient.Address = client.Address;
            existingClient.City = client.City;
            existingClient.State = client.State;
            existingClient.ZipCode = client.ZipCode;
            existingClient.Country = client.Country;
            existingClient.Type = client.Type;
            existingClient.Notes = client.Notes;
            existingClient.IsActive = client.IsActive;
            existingClient.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            
            return Ok(existingClient);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var tenantId = GetCurrentTenantId();
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.ClientId == id && c.TenantId == tenantId);
            
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveClients()
        {
            var tenantId = GetCurrentTenantId();
            var clients = await _context.Clients
                .Where(c => c.TenantId == tenantId && c.IsActive)
                .ToListAsync();
            
            return Ok(clients);
        }
    }
}
