using Microsoft.AspNetCore.Identity.Data;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
using WebApplication1.DTOs.Response.Common;
using LoginRequest = Microsoft.AspNetCore.Identity.Data.LoginRequest;
using RegisterRequest = Microsoft.AspNetCore.Identity.Data.RegisterRequest;

namespace WebApplication1.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest request);
        Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request);
        Task<ApiResponse<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request);
        Task<ApiResponse<string>> ChangePasswordAsync(int userId, ChangePasswordRequest request);
        Task<ApiResponse<string>> LogoutAsync(int userId);
    }
}