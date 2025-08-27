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
    public class EquipmentController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public EquipmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEquipment()
        {
            var tenantId = GetCurrentTenantId();
            var equipment = await _context.Equipment
                .Include(e => e.Tenant)
                .Where(e => e.TenantId == tenantId)
                .ToListAsync();
            
            return Ok(equipment);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEquipment(int id)
        {
            var tenantId = GetCurrentTenantId();
            var equipment = await _context.Equipment
                .Include(e => e.Tenant)
                .FirstOrDefaultAsync(e => e.EquipmentId == id && e.TenantId == tenantId);
            
            if (equipment == null)
            {
                return NotFound();
            }
            
            return Ok(equipment);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEquipment([FromBody] Equipment equipment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Set tenant ID from current context
            equipment.TenantId = GetCurrentTenantId();

            _context.Equipment.Add(equipment);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetEquipment), new { id = equipment.EquipmentId }, equipment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEquipment(int id, [FromBody] Equipment equipment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenantId = GetCurrentTenantId();
            var existingEquipment = await _context.Equipment
                .FirstOrDefaultAsync(e => e.EquipmentId == id && e.TenantId == tenantId);
            
            if (existingEquipment == null)
            {
                return NotFound();
            }

            // Update properties
            existingEquipment.Name = equipment.Name;
            existingEquipment.Description = equipment.Description;
            existingEquipment.SerialNumber = equipment.SerialNumber;
            existingEquipment.Model = equipment.Model;
            existingEquipment.Manufacturer = equipment.Manufacturer;
            existingEquipment.PurchaseDate = equipment.PurchaseDate;
            existingEquipment.PurchasePrice = equipment.PurchasePrice;
            existingEquipment.CurrentValue = equipment.CurrentValue;
            existingEquipment.Status = equipment.Status;
            existingEquipment.LastMaintenanceDate = equipment.LastMaintenanceDate;
            existingEquipment.NextMaintenanceDate = equipment.NextMaintenanceDate;
            existingEquipment.MaintenanceNotes = equipment.MaintenanceNotes;
            existingEquipment.IsActive = equipment.IsActive;
            existingEquipment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            
            return Ok(existingEquipment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipment(int id)
        {
            var tenantId = GetCurrentTenantId();
            var equipment = await _context.Equipment
                .FirstOrDefaultAsync(e => e.EquipmentId == id && e.TenantId == tenantId);
            
            if (equipment == null)
            {
                return NotFound();
            }

            _context.Equipment.Remove(equipment);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}
