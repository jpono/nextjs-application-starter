using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ConstructionSaaSBackend.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Extract tenant ID from the request
            // This could be from various sources: header, route, subdomain, JWT token, etc.
            // For this implementation, we'll use a header "X-Tenant-Id"
            
            if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantIdValue) &&
                int.TryParse(tenantIdValue, out int tenantId))
            {
                context.Items["TenantId"] = tenantId;
            }
            else
            {
                // If no tenant ID is provided, try to get it from the authenticated user
                var user = context.User;
                if (user.Identity?.IsAuthenticated == true)
                {
                    var tenantIdClaim = user.FindFirst("TenantId");
                    if (tenantIdClaim != null && int.TryParse(tenantIdClaim.Value, out int userTenantId))
                    {
                        context.Items["TenantId"] = userTenantId;
                    }
                }
            }

            // If no tenant ID is found, you might want to handle this case
            // For now, we'll proceed and let the controllers handle authorization
            await _next(context);
        }
    }
}
