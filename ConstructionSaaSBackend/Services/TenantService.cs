using ConstructionSaaSBackend.Data;
using ConstructionSaaSBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConstructionSaaSBackend.Services
{
    public interface ITenantService
    {
        Task<Tenant?> GetTenantByIdAsync(int tenantId);
        Task<IEnumerable<Tenant>> GetAllTenantsAsync();
        Task<Tenant> CreateTenantAsync(Tenant tenant);
        Task<Tenant> UpdateTenantAsync(int tenantId, Tenant tenant);
        Task<bool> DeleteTenantAsync(int tenantId);
        Task<bool> TenantExistsAsync(int tenantId);
    }

    public class TenantService : ITenantService
    {
        private readonly ApplicationDbContext _context;

        public TenantService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Tenant?> GetTenantByIdAsync(int tenantId)
        {
            return await _context.Tenants
                .FirstOrDefaultAsync(t => t.TenantId == tenantId);
        }

        public async Task<IEnumerable<Tenant>> GetAllTenantsAsync()
        {
            return await _context.Tenants
                .ToListAsync();
        }

        public async Task<Tenant> CreateTenantAsync(Tenant tenant)
        {
            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync();
            return tenant;
        }

        public async Task<Tenant> UpdateTenantAsync(int tenantId, Tenant tenant)
        {
            var existingTenant = await _context.Tenants.FindAsync(tenantId);
            if (existingTenant == null)
            {
                throw new KeyNotFoundException($"Tenant with ID {tenantId} not found.");
            }

            existingTenant.Name = tenant.Name;
            existingTenant.Description = tenant.Description;
            existingTenant.IsActive = tenant.IsActive;

            await _context.SaveChangesAsync();
            return existingTenant;
        }

        public async Task<bool> DeleteTenantAsync(int tenantId)
        {
            var tenant = await _context.Tenants.FindAsync(tenantId);
            if (tenant == null)
            {
                return false;
            }

            _context.Tenants.Remove(tenant);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> TenantExistsAsync(int tenantId)
        {
            return await _context.Tenants
                .AnyAsync(t => t.TenantId == tenantId);
        }
    }
}
