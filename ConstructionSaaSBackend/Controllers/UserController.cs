using ConstructionSaaSBackend.Data;
using ConstructionSaaSBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public class UserController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var tenantId = GetCurrentTenantId();
            var users = await _context.Users
                .Include(u => u.Tenant)
                .Where(u => u.TenantId == tenantId)
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    u.FullName,
                    u.CreatedAt,
                    u.IsActive,
                    u.TenantId
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var tenantId = GetCurrentTenantId();
            var user = await _context.Users
                .Include(u => u.Tenant)
                .FirstOrDefaultAsync(u => u.Id == id && u.TenantId == tenantId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.FirstName,
                user.LastName,
                user.FullName,
                user.CreatedAt,
                user.IsActive,
                user.TenantId
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenantId = GetCurrentTenantId();

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                TenantId = tenantId,
                IsActive = request.IsActive
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.FirstName,
                user.LastName,
                user.FullName,
                user.CreatedAt,
                user.IsActive,
                user.TenantId
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenantId = GetCurrentTenantId();
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.TenantId == tenantId);

            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.IsActive = request.IsActive;

            if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
            {
                user.Email = request.Email;
                user.UserName = request.Email;
            }

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.FirstName,
                user.LastName,
                user.FullName,
                user.CreatedAt,
                user.IsActive,
                user.TenantId
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var tenantId = GetCurrentTenantId();
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.TenantId == tenantId);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveUsers()
        {
            var tenantId = GetCurrentTenantId();
            var users = await _context.Users
                .Where(u => u.TenantId == tenantId && u.IsActive)
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    u.FullName,
                    u.CreatedAt,
                    u.IsActive,
                    u.TenantId
                })
                .ToListAsync();

            return Ok(users);
        }
    }

    public class CreateUserRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    public class UpdateUserRequest
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
