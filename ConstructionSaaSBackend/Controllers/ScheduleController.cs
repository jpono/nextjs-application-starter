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
    public class ScheduleController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public ScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSchedules()
        {
            var tenantId = GetCurrentTenantId();
            var schedules = await _context.Schedules
                .Include(s => s.Tenant)
                .Include(s => s.Project)
                .Include(s => s.Employee)
                .Include(s => s.Equipment)
                .Where(s => s.TenantId == tenantId)
                .ToListAsync();
            
            return Ok(schedules);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchedule(int id)
        {
            var tenantId = GetCurrentTenantId();
            var schedule = await _context.Schedules
                .Include(s => s.Tenant)
                .Include(s => s.Project)
                .Include(s => s.Employee)
                .Include(s => s.Equipment)
                .FirstOrDefaultAsync(s => s.ScheduleId == id && s.TenantId == tenantId);
            
            if (schedule == null)
            {
                return NotFound();
            }
            
            return Ok(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchedule([FromBody] Schedule schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Set tenant ID from current context
            schedule.TenantId = GetCurrentTenantId();

            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetSchedule), new { id = schedule.ScheduleId }, schedule);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule(int id, [FromBody] Schedule schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenantId = GetCurrentTenantId();
            var existingSchedule = await _context.Schedules
                .FirstOrDefaultAsync(s => s.ScheduleId == id && s.TenantId == tenantId);
            
            if (existingSchedule == null)
            {
                return NotFound();
            }

            // Update properties
            existingSchedule.Title = schedule.Title;
            existingSchedule.Description = schedule.Description;
            existingSchedule.StartDateTime = schedule.StartDateTime;
            existingSchedule.EndDateTime = schedule.EndDateTime;
            existingSchedule.IsAllDay = schedule.IsAllDay;
            existingSchedule.ProjectId = schedule.ProjectId;
            existingSchedule.EmployeeId = schedule.EmployeeId;
            existingSchedule.EquipmentId = schedule.EquipmentId;
            existingSchedule.Type = schedule.Type;
            existingSchedule.Status = schedule.Status;
            existingSchedule.Location = schedule.Location;
            existingSchedule.Notes = schedule.Notes;
            existingSchedule.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            
            return Ok(existingSchedule);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var tenantId = GetCurrentTenantId();
            var schedule = await _context.Schedules
                .FirstOrDefaultAsync(s => s.ScheduleId == id && s.TenantId == tenantId);
            
            if (schedule == null)
            {
                return NotFound();
            }

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetSchedulesByProject(int projectId)
        {
            var tenantId = GetCurrentTenantId();
            var schedules = await _context.Schedules
                .Include(s => s.Project)
                .Include(s => s.Employee)
                .Include(s => s.Equipment)
                .Where(s => s.TenantId == tenantId && s.ProjectId == projectId)
                .ToListAsync();
            
            return Ok(schedules);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetSchedulesByEmployee(int employeeId)
        {
            var tenantId = GetCurrentTenantId();
            var schedules = await _context.Schedules
                .Include(s => s.Project)
                .Include(s => s.Employee)
                .Include(s => s.Equipment)
                .Where(s => s.TenantId == tenantId && s.EmployeeId == employeeId)
                .ToListAsync();
            
            return Ok(schedules);
        }

        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetSchedulesByDate(DateTime date)
        {
            var tenantId = GetCurrentTenantId();
            var startOfDay = date.Date;
            var endOfDay = date.Date.AddDays(1).AddTicks(-1);
            
            var schedules = await _context.Schedules
                .Include(s => s.Project)
                .Include(s => s.Employee)
                .Include(s => s.Equipment)
                .Where(s => s.TenantId == tenantId && 
                           s.StartDateTime >= startOfDay && 
                           s.StartDateTime <= endOfDay)
                .ToListAsync();
            
            return Ok(schedules);
        }
    }
}
