using WebApplication1.enums;

namespace WebApplication1.Models
{
    public class Trade : BaseEntity
{
    public string TradeNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
 
    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;
 
    public int IncotermId { get; set; }
    public Incoterm Incoterm { get; set; } = null!;
 
    public string BuyerName { get; set; } = string.Empty;
    public string SellerName { get; set; } = string.Empty;
    public string OriginCountry { get; set; } = string.Empty;
    public string DestinationCountry { get; set; } = string.Empty;
    public string Commodity { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public decimal TotalValue { get; set; }
    public string Currency { get; set; } = "USD";
    public TradeStatus Status { get; set; } = TradeStatus.Pending;
    public DateTime TradeDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? Documents { get; set; }
    public string? Notes { get; set; }
 
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
}