
using Microsoft.AspNetCore.Identity.Data;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
using WebApplication1.DTOs.Response.Common;

namespace WebApplication1.Application.Interfaces
{
    public interface ICompanyService
    {
        Task<ApiResponse<CompanyResponse>> GetByIdAsync(int id);
        Task<ApiResponse<IEnumerable<CompanyResponse>>> GetAllAsync();
        Task<ApiResponse<CompanyResponse>> CreateAsync(CreateCompanyRequest request);
        Task<ApiResponse<CompanyResponse>> UpdateAsync(int id, UpdateCompanyRequest request);
        Task<ApiResponse<string>> DeleteAsync(int id);
    }
}