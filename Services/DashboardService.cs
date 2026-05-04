using WebApplication1.Application.Interfaces;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
using WebApplication1.DTOs.Response.Common;
using WebApplication1.enums;
using WebApplication1.Models;
using WebApplication1.Interfaces.Common;

namespace  WebApplication1.Services;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _uow;
    public DashboardService(IUnitOfWork uow) => _uow = uow;

    public async Task<ApiResponse<DashboardResponse>> GetDashboardAsync()
    {
        var companies = await _uow.Companies.GetAllAsync();
        var allTrades = await _uow.Trades.GetAllAsync();
        var allInvoices = await _uow.Invoices.GetAllAsync();
        var allSubs = await _uow.Subscriptions.GetAllAsync();

        var tradeList = allTrades.ToList();
        var invoiceList = allInvoices.ToList();

        var recentTrades = tradeList.OrderByDescending(t => t.CreatedAt).Take(5)
            .Select(t => new TradeResponse(
                t.Id, t.TradeNumber, t.Description, t.CompanyId, t.Company?.Name ?? "",
                t.IncotermId, t.Incoterm?.Code ?? "", t.BuyerName, t.SellerName,
                t.OriginCountry, t.DestinationCountry, t.Commodity,
                t.Quantity, t.Unit, t.UnitPrice, t.TotalValue, t.Currency,
                t.Status.ToString(), t.TradeDate, t.DeliveryDate, t.Notes, t.CreatedAt
            )).ToList();

        var recentInvoices = invoiceList.OrderByDescending(i => i.CreatedAt).Take(5)
            .Select(i => new InvoiceResponse(
                i.Id, i.InvoiceNumber, i.CompanyId, i.Company?.Name ?? "",
                i.TradeId, i.Trade?.TradeNumber, i.SubscriptionId,
                i.SubTotal, i.TaxPercent, i.TaxAmount, i.Discount, i.TotalAmount,
                i.Currency, i.Status.ToString(), i.IssueDate, i.DueDate,
                i.PaidDate, i.Notes, i.CreatedAt
            )).ToList();

        var dashboard = new DashboardResponse(
            TotalCompanies: companies.Count(),
            ActiveSubscriptions: allSubs.Count(s => s.Status == SubscriptionStatus.Active),
            TotalTrades: tradeList.Count,
            PendingTrades: tradeList.Count(t => t.Status == TradeStatus.Pending),
            TotalRevenue: invoiceList.Where(i => i.Status == InvoiceStatus.Paid).Sum(i => i.TotalAmount),
            OverdueInvoices: invoiceList.Count(i => i.Status == InvoiceStatus.Overdue),
            RecentTrades: recentTrades,
            RecentInvoices: recentInvoices
        );

        return new ApiResponse<DashboardResponse>(true, "Success.", dashboard);
    }
}