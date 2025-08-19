using System.ComponentModel.DataAnnotations;

namespace ConstructionSaaSBackend.Models
{
    public class Document
    {
        public int DocumentId { get; set; }
        
        [Required]
        public int TenantId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string FileName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string OriginalFileName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string ContentType { get; set; } = string.Empty;
        
        [Range(0, long.MaxValue)]
        public long FileSize { get; set; }
        
        public int? ProjectId { get; set; }
        
        public DocumentCategory Category { get; set; } = DocumentCategory.General;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [StringLength(100)]
        public string? UploadedBy { get; set; }
        
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual Tenant Tenant { get; set; } = null!;
        public virtual Project? Project { get; set; }
    }
    
    public enum DocumentCategory
    {
        General,
        Contract,
        Blueprint,
        Permit,
        Invoice,
        Receipt,
        Photo,
        Report,
        Specification,
        Other
    }
}
