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
    public class ReportController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReports()
        {
            var tenantId = GetCurrentTenantId();
            var reports = await _context.Reports
                .Include(r => r.Tenant)
                .Include(r => r.Project)
                .Where(r => r.TenantId == tenantId)
                .ToListAsync();
            
            return Ok(reports);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReport(int id)
        {
            var tenantId = GetCurrentTenantId();
            var report = await _context.Reports
                .Include(r => r.Tenant)
                .Include(r => r.Project)
                .FirstOrDefaultAsync(r => r.ReportId == id && r.TenantId == tenantId);
            
            if (report == null)
            {
                return NotFound();
            }
            
            return Ok(report);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] Report report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Set tenant ID from current context
            report.TenantId = GetCurrentTenantId();

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetReport), new { id = report.ReportId }, report);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReport(int id, [FromBody] Report report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenantId = GetCurrentTenantId();
            var existingReport = await _context.Reports
                .FirstOrDefaultAsync(r => r.ReportId == id && r.TenantId == tenantId);
            
            if (existingReport == null)
            {
                return NotFound();
            }

            // Update properties
            existingReport.Title = report.Title;
            existingReport.Description = report.Description;
            existingReport.Type = report.Type;
            existingReport.ProjectId = report.ProjectId;
            existingReport.StartDate = report.StartDate;
            existingReport.EndDate = report.EndDate;
            existingReport.Data = report.Data;
            existingReport.GeneratedBy = report.GeneratedBy;
            existingReport.IsActive = report.IsActive;
            existingReport.GeneratedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            
            return Ok(existingReport);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var tenantId = GetCurrentTenantId();
            var report = await _context.Reports
                .FirstOrDefaultAsync(r => r.ReportId == id && r.TenantId == tenantId);
            
            if (report == null)
            {
                return NotFound();
            }

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}
