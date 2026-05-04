using Microsoft.AspNetCore.Identity.Data;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
using WebApplication1.DTOs.Response.Common;

namespace WebApplication1.Application.Interfaces
{
public interface ISubscriptionService
{
    Task<ApiResponse<SubscriptionResponse>> GetByIdAsync(int id);
    Task<ApiResponse<IEnumerable<SubscriptionResponse>>> GetByCompanyAsync(int companyId);
    Task<ApiResponse<SubscriptionResponse>> GetActiveSubscriptionAsync(int companyId);
    Task<ApiResponse<SubscriptionResponse>> CreateAsync(CreateSubscriptionRequest request);
    Task<ApiResponse<SubscriptionResponse>> RenewAsync(RenewSubscriptionRequest request);
    Task<ApiResponse<SubscriptionResponse>> UpgradeAsync(UpgradeSubscriptionRequest request);
    Task<ApiResponse<string>> CancelAsync(int id);
    Task<ApiResponse<IEnumerable<SubscriptionPlanResponse>>> GetPlansAsync();
}
}