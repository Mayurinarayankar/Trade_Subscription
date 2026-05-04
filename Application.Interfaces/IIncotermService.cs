using Microsoft.AspNetCore.Identity.Data;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
using WebApplication1.DTOs.Response.Common;

namespace WebApplication1.Application.Interfaces
{
    public interface IIncotermService
{
    Task<ApiResponse<IEnumerable<IncotermResponse>>> GetAllAsync();
    Task<ApiResponse<IncotermResponse>> GetByIdAsync(int id);
    Task<ApiResponse<IncotermResponse>> CreateAsync(CreateIncotermRequest request);
}
}