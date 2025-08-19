using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ConstructionSaaSBackend.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public int TenantId { get; set; }
        
        [StringLength(50)]
        public string? FirstName { get; set; }
        
        [StringLength(50)]
        public string? LastName { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsActive { get; set; } = true;
        
        // Navigation property
        public virtual Tenant Tenant { get; set; } = null!;
        
        // Full name property
        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}
