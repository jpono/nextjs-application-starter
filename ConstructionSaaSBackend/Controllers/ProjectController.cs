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
    public class ProjectController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var tenantId = GetCurrentTenantId();
            var projects = await _context.Projects
                .Include(p => p.Client)
                .Include(p => p.Tenant)
                .Where(p => p.TenantId == tenantId)
                .ToListAsync();
            
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var tenantId = GetCurrentTenantId();
            var project = await _context.Projects
                .Include(p => p.Client)
                .Include(p => p.Tenant)
                .FirstOrDefaultAsync(p => p.ProjectId == id && p.TenantId == tenantId);
            
            if (project == null)
            {
                return NotFound();
            }
            
            return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Set tenant ID from current context
            project.TenantId = GetCurrentTenantId();

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetProject), new { id = project.ProjectId }, project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenantId = GetCurrentTenantId();
            var existingProject = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectId == id && p.TenantId == tenantId);
            
            if (existingProject == null)
            {
                return NotFound();
            }

            // Update properties
            existingProject.Name = project.Name;
            existingProject.Description = project.Description;
            existingProject.ClientId = project.ClientId;
            existingProject.Address = project.Address;
            existingProject.StartDate = project.StartDate;
            existingProject.EndDate = project.EndDate;
            existingProject.ActualEndDate = project.ActualEndDate;
            existingProject.Budget = project.Budget;
            existingProject.ActualCost = project.ActualCost;
            existingProject.Status = project.Status;
            existingProject.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            
            return Ok(existingProject);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var tenantId = GetCurrentTenantId();
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectId == id && p.TenantId == tenantId);
            
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetProjectsByClient(int clientId)
        {
            var tenantId = GetCurrentTenantId();
            var projects = await _context.Projects
                .Include(p => p.Client)
                .Where(p => p.TenantId == tenantId && p.ClientId == clientId)
                .ToListAsync();
            
            return Ok(projects);
        }
    }
}
