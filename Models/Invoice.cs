using WebApplication1.enums;

namespace WebApplication1.Models
{
   public class Invoice : BaseEntity
{
    public string InvoiceNumber { get; set; } = string.Empty;
 
    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;
 
    public int? TradeId { get; set; }
    public Trade? Trade { get; set; }
 
    public int? SubscriptionId { get; set; }
    public Subscription? Subscription { get; set; }
 
    public decimal SubTotal { get; set; }
    public decimal TaxPercent { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = "USD";
 
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public string? Notes { get; set; }
}
}