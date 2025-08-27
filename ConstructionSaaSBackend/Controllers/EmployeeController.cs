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
    public class EmployeeController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var tenantId = GetCurrentTenantId();
            var employees = await _context.Employees
                .Include(e => e.Tenant)
                .Where(e => e.TenantId == tenantId)
                .ToListAsync();
            
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var tenantId = GetCurrentTenantId();
            var employee = await _context.Employees
                .Include(e => e.Tenant)
                .FirstOrDefaultAsync(e => e.EmployeeId == id && e.TenantId == tenantId);
            
            if (employee == null)
            {
                return NotFound();
            }
            
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Set tenant ID from current context
            employee.TenantId = GetCurrentTenantId();

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenantId = GetCurrentTenantId();
            var existingEmployee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == id && e.TenantId == tenantId);
            
            if (existingEmployee == null)
            {
                return NotFound();
            }

            // Update properties
            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.Email = employee.Email;
            existingEmployee.PhoneNumber = employee.PhoneNumber;
            existingEmployee.Address = employee.Address;
            existingEmployee.Position = employee.Position;
            existingEmployee.Department = employee.Department;
            existingEmployee.HourlyRate = employee.HourlyRate;
            existingEmployee.HireDate = employee.HireDate;
            existingEmployee.TerminationDate = employee.TerminationDate;
            existingEmployee.IsActive = employee.IsActive;
            existingEmployee.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            
            return Ok(existingEmployee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var tenantId = GetCurrentTenantId();
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == id && e.TenantId == tenantId);
            
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveEmployees()
        {
            var tenantId = GetCurrentTenantId();
            var employees = await _context.Employees
                .Where(e => e.TenantId == tenantId && e.IsActive)
                .ToListAsync();
            
            return Ok(employees);
        }
    }
}
