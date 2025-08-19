using System.ComponentModel.DataAnnotations;

namespace ConstructionSaaSBackend.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        
        [Required]
        public int TenantId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [Required]
        public int ClientId { get; set; }
        
        [StringLength(500)]
        public string? Address { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public DateTime? ActualEndDate { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal Budget { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal ActualCost { get; set; }
        
        public ProjectStatus Status { get; set; } = ProjectStatus.Planning;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Tenant Tenant { get; set; } = null!;
        public virtual Client Client { get; set; } = null!;
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
    
    public enum ProjectStatus
    {
        Planning,
        InProgress,
        OnHold,
        Completed,
        Cancelled
    }
}
