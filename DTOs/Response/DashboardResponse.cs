namespace WebApplication1.DTOs.Response
{
    public record DashboardResponse(
    int TotalCompanies,
    int ActiveSubscriptions,
    int TotalTrades,
    int PendingTrades,
    decimal TotalRevenue,
    int OverdueInvoices,
    List<TradeResponse> RecentTrades,
    List<InvoiceResponse> RecentInvoices
);
}