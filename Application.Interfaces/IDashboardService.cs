using Microsoft.AspNetCore.Identity.Data;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
using WebApplication1.DTOs.Response.Common;

namespace WebApplication1.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<ApiResponse<DashboardResponse>> GetDashboardAsync();
    }
}