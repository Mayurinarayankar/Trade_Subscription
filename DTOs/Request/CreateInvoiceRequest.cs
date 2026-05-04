namespace WebApplication1.DTOs.Request
{
public record CreateInvoiceRequest(
    int CompanyId,
    int? TradeId,
    int? SubscriptionId,
    decimal SubTotal,
    decimal TaxPercent,
    decimal Discount,
    string Currency,
    DateTime IssueDate,
    DateTime DueDate,
    string? Notes
);
 
public record UpdateInvoiceStatusRequest(string Status);
 
}