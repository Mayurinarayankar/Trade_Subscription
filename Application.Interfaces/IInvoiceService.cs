using Microsoft.AspNetCore.Identity.Data;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
using WebApplication1.DTOs.Response.Common;

namespace WebApplication1.Application.Interfaces
{
    public interface IInvoiceService
    {
        Task<ApiResponse<InvoiceResponse>> GetByIdAsync(int id);
        Task<ApiResponse<IEnumerable<InvoiceResponse>>> GetByCompanyAsync(int companyId);
        Task<ApiResponse<InvoiceResponse>> CreateAsync(CreateInvoiceRequest request);
        Task<ApiResponse<InvoiceResponse>> UpdateStatusAsync(int id, UpdateInvoiceStatusRequest request);
        Task<ApiResponse<string>> DeleteAsync(int id);
    }
}