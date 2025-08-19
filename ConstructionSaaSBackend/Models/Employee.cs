using System.ComponentModel.DataAnnotations;

namespace ConstructionSaaSBackend.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        
        [Required]
        public int TenantId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Phone]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        
        [StringLength(500)]
        public string? Address { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Position { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? Department { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal HourlyRate { get; set; }
        
        public DateTime HireDate { get; set; }
        
        public DateTime? TerminationDate { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Tenant Tenant { get; set; } = null!;
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        
        // Full name property
        public string FullName => $"{FirstName} {LastName}";
    }
}
