using System.ComponentModel.DataAnnotations;

namespace ConstructionSaaSBackend.Models
{
    public class Equipment
    {
        public int EquipmentId { get; set; }
        
        [Required]
        public int TenantId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Required]
        [StringLength(50)]
        public string SerialNumber { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? Model { get; set; }
        
        [StringLength(50)]
        public string? Manufacturer { get; set; }
        
        public DateTime? PurchaseDate { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal PurchasePrice { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal CurrentValue { get; set; }
        
        public EquipmentStatus Status { get; set; } = EquipmentStatus.Available;
        
        public DateTime? LastMaintenanceDate { get; set; }
        
        public DateTime? NextMaintenanceDate { get; set; }
        
        [StringLength(1000)]
        public string? MaintenanceNotes { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Tenant Tenant { get; set; } = null!;
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
    
    public enum EquipmentStatus
    {
        Available,
        InUse,
        Maintenance,
        OutOfService,
        Retired
    }
}
