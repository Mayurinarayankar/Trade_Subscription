namespace WebApplication1.DTOs.Request
{
    public record CreateTradeRequest(
    string Description,
    int IncotermId,
    string BuyerName,
    string SellerName,
    string OriginCountry,
    string DestinationCountry,
    string Commodity,
    decimal Quantity,
    string Unit,
    decimal UnitPrice,
    string Currency,
    DateTime TradeDate,
    DateTime? DeliveryDate,
    string? Notes);
}