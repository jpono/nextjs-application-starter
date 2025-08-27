using Microsoft.AspNetCore.Mvc;

namespace ConstructionSaaSBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected int GetCurrentTenantId()
        {
            if (HttpContext.Items.TryGetValue("TenantId", out var tenantId) && tenantId is int id)
            {
                return id;
            }
            
            throw new UnauthorizedAccessException("Tenant context not available");
        }
        
        protected string GetCurrentUserId()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User not authenticated");
            }
            
            return userId;
        }
    }
}
