using WebApplication1.Application.Interfaces;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
using WebApplication1.DTOs.Response.Common;
using WebApplication1.enums;
using WebApplication1.Models;
using WebApplication1.Interfaces.Common;

namespace  WebApplication1.Services;

public class TradeService : ITradeService
{
    private readonly IUnitOfWork _uow;

    public TradeService(IUnitOfWork uow) => _uow = uow;

    public async Task<ApiResponse<TradeResponse>> GetByIdAsync(int id)
    {
        var trade = await _uow.Trades.GetByIdAsync(id);
        if (trade == null) return new ApiResponse<TradeResponse>(false, "Trade not found.", null);
        return new ApiResponse<TradeResponse>(true, "Success.", MapResponse(trade));
    }

    public async Task<ApiResponse<IEnumerable<TradeResponse>>> GetAllAsync()
    {
        var trades = await _uow.Trades.GetAllAsync();
        return new ApiResponse<IEnumerable<TradeResponse>>(true, "Success.", trades.Select(MapResponse));
    }

    public async Task<ApiResponse<IEnumerable<TradeResponse>>> GetByCompanyAsync(int companyId)
    {
        var trades = await _uow.Trades.GetByCompanyAsync(companyId);
        return new ApiResponse<IEnumerable<TradeResponse>>(true, "Success.", trades.Select(MapResponse));
    }

    public async Task<ApiResponse<TradeResponse>> CreateAsync(int companyId, CreateTradeRequest request)
    {
        var company = await _uow.Companies.GetByIdAsync(companyId);
        if (company == null) return new ApiResponse<TradeResponse>(false, "Company not found.", null);

        var incoterm = await _uow.Incoterms.GetByIdAsync(request.IncotermId);
        if (incoterm == null) return new ApiResponse<TradeResponse>(false, "Incoterm not found.", null);

        var tradeNumber = await _uow.Trades.GenerateTradeNumberAsync();
        var totalValue = request.Quantity * request.UnitPrice;

        var trade = new Trade
        {
            TradeNumber = tradeNumber,
            Description = request.Description,
            CompanyId = companyId,
            IncotermId = request.IncotermId,
            BuyerName = request.BuyerName,
            SellerName = request.SellerName,
            OriginCountry = request.OriginCountry,
            DestinationCountry = request.DestinationCountry,
            Commodity = request.Commodity,
            Quantity = request.Quantity,
            Unit = request.Unit,
            UnitPrice = request.UnitPrice,
            TotalValue = totalValue,
            Currency = request.Currency,
            TradeDate = request.TradeDate,
            DeliveryDate = request.DeliveryDate,
            Notes = request.Notes,
            Status = TradeStatus.Pending
        };

        await _uow.Trades.AddAsync(trade);
        await _uow.SaveChangesAsync();

        trade.Company = company;
        trade.Incoterm = incoterm;

        return new ApiResponse<TradeResponse>(true, "Trade created successfully.", MapResponse(trade));
    }

    public async Task<ApiResponse<TradeResponse>> UpdateAsync(int id, UpdateTradeRequest request)
    {
        var trade = await _uow.Trades.GetByIdAsync(id);
        if (trade == null) return new ApiResponse<TradeResponse>(false, "Trade not found.", null);

        trade.Description = request.Description;
        trade.IncotermId = request.IncotermId;
        trade.BuyerName = request.BuyerName;
        trade.SellerName = request.SellerName;
        trade.OriginCountry = request.OriginCountry;
        trade.DestinationCountry = request.DestinationCountry;
        trade.Commodity = request.Commodity;
        trade.Quantity = request.Quantity;
        trade.Unit = request.Unit;
        trade.UnitPrice = request.UnitPrice;
        trade.TotalValue = request.Quantity * request.UnitPrice;
        trade.Currency = request.Currency;
        trade.TradeDate = request.TradeDate;
        trade.DeliveryDate = request.DeliveryDate;
        trade.Notes = request.Notes;
        trade.UpdatedAt = DateTime.UtcNow;

        await _uow.Trades.UpdateAsync(trade);
        await _uow.SaveChangesAsync();

        return new ApiResponse<TradeResponse>(true, "Trade updated successfully.", MapResponse(trade));
    }

    public async Task<ApiResponse<TradeResponse>> UpdateStatusAsync(int id, UpdateTradeStatusRequest request)
    {
        var trade = await _uow.Trades.GetByIdAsync(id);
        if (trade == null) return new ApiResponse<TradeResponse>(false, "Trade not found.", null);

        if (!Enum.TryParse<TradeStatus>(request.Status, true, out var status))
            return new ApiResponse<TradeResponse>(false, "Invalid status value.", null);

        trade.Status = status;
        trade.UpdatedAt = DateTime.UtcNow;
        await _uow.Trades.UpdateAsync(trade);
        await _uow.SaveChangesAsync();

        return new ApiResponse<TradeResponse>(true, "Trade status updated.", MapResponse(trade));
    }

    public async Task<ApiResponse<string>> DeleteAsync(int id)
    {
        var exists = await _uow.Trades.ExistsAsync(id);
        if (!exists) return new ApiResponse<string>(false, "Trade not found.", null);

        await _uow.Trades.DeleteAsync(id);
        await _uow.SaveChangesAsync();
        return new ApiResponse<string>(true, "Trade deleted successfully.", null);
    }

    private TradeResponse MapResponse(Trade t) => new(
        t.Id, t.TradeNumber, t.Description,
        t.CompanyId, t.Company?.Name ?? "",
        t.IncotermId, t.Incoterm?.Code ?? "",
        t.BuyerName, t.SellerName,
        t.OriginCountry, t.DestinationCountry,
        t.Commodity, t.Quantity, t.Unit,
        t.UnitPrice, t.TotalValue, t.Currency,
        t.Status.ToString(), t.TradeDate, t.DeliveryDate,
        t.Notes, t.CreatedAt
    );
}