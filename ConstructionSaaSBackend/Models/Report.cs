using System.ComponentModel.DataAnnotations;

namespace ConstructionSaaSBackend.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        
        [Required]
        public int TenantId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public ReportType Type { get; set; } = ReportType.ProjectSummary;
        
        public int? ProjectId { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        [Required]
        public string Data { get; set; } = string.Empty; // JSON data
        
        [StringLength(100)]
        public string? GeneratedBy { get; set; }
        
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual Tenant Tenant { get; set; } = null!;
        public virtual Project? Project { get; set; }
    }
    
    public enum ReportType
    {
        ProjectSummary,
        FinancialSummary,
        EmployeeHours,
        EquipmentUsage,
        ClientActivity,
        InvoiceStatus,
        ScheduleOverview,
        Custom
    }
}
