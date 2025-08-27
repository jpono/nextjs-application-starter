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
    public class InvoiceController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public InvoiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvoices()
        {
            var tenantId = GetCurrentTenantId();
            var invoices = await _context.Invoices
                .Include(i => i.Tenant)
                .Include(i => i.Client)
                .Include(i => i.Project)
                .Include(i => i.InvoiceItems)
                .Where(i => i.TenantId == tenantId)
                .ToListAsync();
            
            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoice(int id)
        {
            var tenantId = GetCurrentTenantId();
            var invoice = await _context.Invoices
                .Include(i => i.Tenant)
                .Include(i => i.Client)
                .Include(i => i.Project)
                .Include(i => i.InvoiceItems)
                .FirstOrDefaultAsync(i => i.InvoiceId == id && i.TenantId == tenantId);
            
            if (invoice == null)
            {
                return NotFound();
            }
            
            return Ok(invoice);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Set tenant ID from current context
            invoice.TenantId = GetCurrentTenantId();

            // Calculate totals if not provided
            if (invoice.InvoiceItems != null && invoice.InvoiceItems.Any())
            {
                invoice.SubTotal = invoice.InvoiceItems.Sum(item => item.Total);
                invoice.TaxAmount = invoice.SubTotal * (invoice.TaxRate / 100);
                invoice.Total = invoice.SubTotal + invoice.TaxAmount;
            }

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetInvoice), new { id = invoice.InvoiceId }, invoice);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(int id, [FromBody] Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenantId = GetCurrentTenantId();
            var existingInvoice = await _context.Invoices
                .Include(i => i.InvoiceItems)
                .FirstOrDefaultAsync(i => i.InvoiceId == id && i.TenantId == tenantId);
            
            if (existingInvoice == null)
            {
                return NotFound();
            }

            // Update properties
            existingInvoice.InvoiceNumber = invoice.InvoiceNumber;
            existingInvoice.ClientId = invoice.ClientId;
            existingInvoice.ProjectId = invoice.ProjectId;
            existingInvoice.InvoiceDate = invoice.InvoiceDate;
            existingInvoice.DueDate = invoice.DueDate;
            existingInvoice.TaxRate = invoice.TaxRate;
            existingInvoice.AmountPaid = invoice.AmountPaid;
            existingInvoice.Status = invoice.Status;
            existingInvoice.Notes = invoice.Notes;
            existingInvoice.UpdatedAt = DateTime.UtcNow;

            // Update invoice items
            if (invoice.InvoiceItems != null)
            {
                // Remove existing items
                _context.InvoiceItems.RemoveRange(existingInvoice.InvoiceItems);
                
                // Add new items
                foreach (var item in invoice.InvoiceItems)
                {
                    item.InvoiceId = id;
                    _context.InvoiceItems.Add(item);
                }

                // Recalculate totals
                existingInvoice.SubTotal = invoice.InvoiceItems.Sum(item => item.Total);
                existingInvoice.TaxAmount = existingInvoice.SubTotal * (existingInvoice.TaxRate / 100);
                existingInvoice.Total = existingInvoice.SubTotal + existingInvoice.TaxAmount;
            }

            await _context.SaveChangesAsync();
            
            return Ok(existingInvoice);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var tenantId = GetCurrentTenantId();
            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.InvoiceId == id && i.TenantId == tenantId);
            
            if (invoice == null)
            {
                return NotFound();
            }

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetInvoicesByClient(int clientId)
        {
            var tenantId = GetCurrentTenantId();
            var invoices = await _context.Invoices
                .Include(i => i.Client)
                .Include(i => i.Project)
                .Where(i => i.TenantId == tenantId && i.ClientId == clientId)
                .ToListAsync();
            
            return Ok(invoices);
        }

        [HttpPost("{id}/pay")]
        public async Task<IActionResult> RecordPayment(int id, [FromBody] decimal amount)
        {
            var tenantId = GetCurrentTenantId();
            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.InvoiceId == id && i.TenantId == tenantId);
            
            if (invoice == null)
            {
                return NotFound();
            }

            invoice.AmountPaid += amount;
            
            // Update status if fully paid
            if (invoice.AmountPaid >= invoice.Total)
            {
                invoice.Status = InvoiceStatus.Paid;
            }
            else if (invoice.DueDate < DateTime.UtcNow)
            {
                invoice.Status = InvoiceStatus.Overdue;
            }
            else
            {
                invoice.Status = InvoiceStatus.Sent;
            }

            invoice.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            
            return Ok(invoice);
        }
    }
}
