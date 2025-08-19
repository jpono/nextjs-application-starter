using System.ComponentModel.DataAnnotations;

namespace ConstructionSaaSBackend.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }
        
        [Required]
        public int TenantId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public DateTime StartDateTime { get; set; }
        
        public DateTime EndDateTime { get; set; }
        
        public bool IsAllDay { get; set; } = false;
        
        public int? ProjectId { get; set; }
        
        public int? EmployeeId { get; set; }
        
        public int? EquipmentId { get; set; }
        
        public ScheduleType Type { get; set; } = ScheduleType.Task;
        
        public ScheduleStatus Status { get; set; } = ScheduleStatus.Scheduled;
        
        [StringLength(500)]
        public string? Location { get; set; }
        
        [StringLength(1000)]
        public string? Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Tenant Tenant { get; set; } = null!;
        public virtual Project? Project { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual Equipment? Equipment { get; set; }
    }
    
    public enum ScheduleType
    {
        Task,
        Meeting,
        Maintenance,
        Inspection,
        Delivery,
        Other
    }
    
    public enum ScheduleStatus
    {
        Scheduled,
        InProgress,
        Completed,
        Cancelled,
        Postponed
    }
}
