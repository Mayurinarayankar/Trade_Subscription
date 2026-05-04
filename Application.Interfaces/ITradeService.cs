using Microsoft.AspNetCore.Identity.Data;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
using WebApplication1.DTOs.Response.Common;

namespace WebApplication1.Application.Interfaces
{
public interface ITradeService
{
    Task<ApiResponse<TradeResponse>> GetByIdAsync(int id);
    Task<ApiResponse<IEnumerable<TradeResponse>>> GetAllAsync();
    Task<ApiResponse<IEnumerable<TradeResponse>>> GetByCompanyAsync(int companyId);
    Task<ApiResponse<TradeResponse>> CreateAsync(int companyId, CreateTradeRequest request);
    Task<ApiResponse<TradeResponse>> UpdateAsync(int id, UpdateTradeRequest request);
    Task<ApiResponse<TradeResponse>> UpdateStatusAsync(int id, UpdateTradeStatusRequest request);
    Task<ApiResponse<string>> DeleteAsync(int id);
}
 
}