using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ConstructionSaaSBackend.Models;

namespace ConstructionSaaSBackend.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships
            ConfigureRelationships(builder);
            
            // Configure global query filters for multi-tenancy
            ConfigureGlobalFilters(builder);
            
            // Configure indexes
            ConfigureIndexes(builder);
        }

        private void ConfigureRelationships(ModelBuilder builder)
        {
            // ApplicationUser - Tenant relationship
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Tenant)
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Project relationships
            builder.Entity<Project>()
                .HasOne(p => p.Tenant)
                .WithMany(t => t.Projects)
                .HasForeignKey(p => p.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Project>()
                .HasOne(p => p.Client)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee - Tenant relationship
            builder.Entity<Employee>()
                .HasOne(e => e.Tenant)
                .WithMany(t => t.Employees)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Equipment - Tenant relationship
            builder.Entity<Equipment>()
                .HasOne(e => e.Tenant)
                .WithMany(t => t.Equipment)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Client - Tenant relationship
            builder.Entity<Client>()
                .HasOne(c => c.Tenant)
                .WithMany(t => t.Clients)
                .HasForeignKey(c => c.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Invoice relationships
            builder.Entity<Invoice>()
                .HasOne(i => i.Tenant)
                .WithMany()
                .HasForeignKey(i => i.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Invoice>()
                .HasOne(i => i.Client)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Invoice>()
                .HasOne(i => i.Project)
                .WithMany(p => p.Invoices)
                .HasForeignKey(i => i.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);

            // InvoiceItem - Invoice relationship
            builder.Entity<InvoiceItem>()
                .HasOne(ii => ii.Invoice)
                .WithMany(i => i.InvoiceItems)
                .HasForeignKey(ii => ii.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Schedule relationships
            builder.Entity<Schedule>()
                .HasOne(s => s.Tenant)
                .WithMany()
                .HasForeignKey(s => s.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Schedule>()
                .HasOne(s => s.Project)
                .WithMany(p => p.Schedules)
                .HasForeignKey(s => s.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Schedule>()
                .HasOne(s => s.Employee)
                .WithMany(e => e.Schedules)
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Schedule>()
                .HasOne(s => s.Equipment)
                .WithMany(e => e.Schedules)
                .HasForeignKey(s => s.EquipmentId)
                .OnDelete(DeleteBehavior.SetNull);

            // Document relationships
            builder.Entity<Document>()
                .HasOne(d => d.Tenant)
                .WithMany()
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Document>()
                .HasOne(d => d.Project)
                .WithMany(p => p.Documents)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);

            // Report relationships
            builder.Entity<Report>()
                .HasOne(r => r.Tenant)
                .WithMany()
                .HasForeignKey(r => r.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Report>()
                .HasOne(r => r.Project)
                .WithMany()
                .HasForeignKey(r => r.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);
        }

        private void ConfigureGlobalFilters(ModelBuilder builder)
        {
            // Apply global query filters for multi-tenancy (row-level security)
            builder.Entity<ApplicationUser>().HasQueryFilter(e => e.TenantId == GetCurrentTenantId());
            builder.Entity<Project>().HasQueryFilter(e => e.TenantId == GetCurrentTenantId());
            builder.Entity<Employee>().HasQueryFilter(e => e.TenantId == GetCurrentTenantId());
            builder.Entity<Equipment>().HasQueryFilter(e => e.TenantId == GetCurrentTenantId());
            builder.Entity<Client>().HasQueryFilter(e => e.TenantId == GetCurrentTenantId());
            builder.Entity<Invoice>().HasQueryFilter(e => e.TenantId == GetCurrentTenantId());
            builder.Entity<Schedule>().HasQueryFilter(e => e.TenantId == GetCurrentTenantId());
            builder.Entity<Document>().HasQueryFilter(e => e.TenantId == GetCurrentTenantId());
            builder.Entity<Report>().HasQueryFilter(e => e.TenantId == GetCurrentTenantId());
        }

        private void ConfigureIndexes(ModelBuilder builder)
        {
            // Add indexes for better performance
            builder.Entity<ApplicationUser>().HasIndex(u => u.TenantId);
            builder.Entity<Project>().HasIndex(p => p.TenantId);
            builder.Entity<Project>().HasIndex(p => p.ClientId);
            builder.Entity<Employee>().HasIndex(e => e.TenantId);
            builder.Entity<Equipment>().HasIndex(e => e.TenantId);
            builder.Entity<Client>().HasIndex(c => c.TenantId);
            builder.Entity<Invoice>().HasIndex(i => i.TenantId);
            builder.Entity<Invoice>().HasIndex(i => i.ClientId);
            builder.Entity<Schedule>().HasIndex(s => s.TenantId);
            builder.Entity<Document>().HasIndex(d => d.TenantId);
            builder.Entity<Report>().HasIndex(r => r.TenantId);
        }

        private int GetCurrentTenantId()
        {
            // Get tenant ID from HTTP context
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.Items.ContainsKey("TenantId") == true)
            {
                return (int)httpContext.Items["TenantId"]!;
            }
            
            // Default to 0 if no tenant context (this should be handled by middleware)
            return 0;
        }

        public override int SaveChanges()
        {
            SetTenantId();
            SetTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetTenantId();
            SetTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SetTenantId()
        {
            var tenantId = GetCurrentTenantId();
            if (tenantId == 0) return;

            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added && e.Entity.GetType().GetProperty("TenantId") != null);

            foreach (var entry in entries)
            {
                entry.Property("TenantId").CurrentValue = tenantId;
            }
        }

        private void SetTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity.GetType().GetProperty("UpdatedAt") != null)
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Added && entry.Entity.GetType().GetProperty("CreatedAt") != null)
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                }
            }
        }
    }
}
