using System.ComponentModel.DataAnnotations;

namespace ConstructionSaaSBackend.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        
        [Required]
        public int TenantId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string InvoiceNumber { get; set; } = string.Empty;
        
        [Required]
        public int ClientId { get; set; }
        
        public int? ProjectId { get; set; }
        
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
        
        public DateTime DueDate { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal SubTotal { get; set; }
        
        [Range(0, 100)]
        public decimal TaxRate { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal TaxAmount { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal Total { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal AmountPaid { get; set; }
        
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;
        
        [StringLength(1000)]
        public string? Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Tenant Tenant { get; set; } = null!;
        public virtual Client Client { get; set; } = null!;
        public virtual Project? Project { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
        
        // Calculated properties
        public decimal Balance => Total - AmountPaid;
        public bool IsOverdue => Status == InvoiceStatus.Sent && DueDate < DateTime.UtcNow;
    }
    
    public class InvoiceItem
    {
        public int InvoiceItemId { get; set; }
        
        [Required]
        public int InvoiceId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        
        [Range(0, double.MaxValue)]
        public decimal Quantity { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal Total { get; set; }
        
        // Navigation property
        public virtual Invoice Invoice { get; set; } = null!;
    }
    
    public enum InvoiceStatus
    {
        Draft,
        Sent,
        Paid,
        Overdue,
        Cancelled
    }
}
