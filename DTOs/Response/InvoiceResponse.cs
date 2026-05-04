namespace WebApplication1.DTOs.Response
{
    public record InvoiceResponse(
    int Id,
    string InvoiceNumber,
    int CompanyId,
    string CompanyName,
    int? TradeId,
    string? TradeNumber,
    int? SubscriptionId,
    decimal SubTotal,
    decimal TaxPercent,
    decimal TaxAmount,
    decimal Discount,
    decimal TotalAmount,
    string Currency,
    string Status,
    DateTime IssueDate,
    DateTime DueDate,
    DateTime? PaidDate,
    string? Notes,
    DateTime CreatedAt
);
}