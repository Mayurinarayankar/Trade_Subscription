namespace WebApplication1.DTOs.Response
{
    public record TradeResponse(
    int Id,
    string TradeNumber,
    string Description,
    int CompanyId,
    string CompanyName,
    int IncotermId,
    string IncotermCode,
    string BuyerName,
    string SellerName,
    string OriginCountry,
    string DestinationCountry,
    string Commodity,
    decimal Quantity,
    string Unit,
    decimal UnitPrice,
    decimal TotalValue,
    string Currency,
    string Status,
    DateTime TradeDate,
    DateTime? DeliveryDate,
    string? Notes,
    DateTime CreatedAt
);
 
}