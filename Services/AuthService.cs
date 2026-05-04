using WebApplication1.Application.Interfaces;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
using WebApplication1.DTOs.Response.Common;
using WebApplication1.enums;
using WebApplication1.Interfaces.Common;
using WebApplication1.Models;

namespace WebApplication1.Services
{

    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly ITokenService _tokenService;

        public AuthService(IUnitOfWork uow, ITokenService tokenService)
        {
            _uow = uow;
            _tokenService = tokenService;
        }

        // public async Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest request)
        // {
        //     var existingUser = await _uow.Users.GetByEmailAsync(request.Email);
        //     if (existingUser != null)
        //         return new ApiResponse<AuthResponse>(false, "Email already registered.", null);

        //     // Create company first
        //     var company = new Company
        //     {
        //         Name = request.CompanyName,
        //         Email = request.CompanyEmail,
        //         Phone = request.Phone,
        //         Address = request.Address,
        //         Country = request.Country,
        //         TaxNumber = request.TaxNumber
        //     };
        //     await _uow.Companies.AddAsync(company);
        //     await _uow.SaveChangesAsync();

        //     // Create admin user
        //     var user = new User
        //     {
        //         FirstName = request.FirstName,
        //         LastName = request.LastName,
        //         Email = request.Email,
        //         PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
        //         Role = UserRole.Admin,
        //         CompanyId = company.Id
        //     };
        //     await _uow.Users.AddAsync(user);

        //     // Generate tokens
        //     var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email, user.Role.ToString());
        //     var refreshToken = _tokenService.GenerateRefreshToken();
        //     user.RefreshToken = refreshToken;
        //     user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        //     await _uow.SaveChangesAsync();

        //     return new ApiResponse<AuthResponse>(true, "Registration successful.", new AuthResponse(
        //         accessToken, refreshToken, DateTime.UtcNow.AddHours(1),
        //         MapUserResponse(user, company.Name)
        //     ));
        // }


        public async Task<ApiResponse<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var user = await _uow.Users.GetByRefreshTokenAsync(request.RefreshToken);
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                return new ApiResponse<AuthResponse>(false, "Invalid or expired refresh token.", null);

            var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email, user.Role.ToString());
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _uow.Users.UpdateAsync(user);
            await _uow.SaveChangesAsync();

            return new ApiResponse<AuthResponse>(true, "Token refreshed.", new AuthResponse(
                accessToken, newRefreshToken, DateTime.UtcNow.AddHours(1),
                MapUserResponse(user, user.Company?.Name ?? "")
            ));
        }

        public async Task<ApiResponse<string>> ChangePasswordAsync(int userId, ChangePasswordRequest request)
        {
            var user = await _uow.Users.GetByIdAsync(userId);
            if (user == null) return new ApiResponse<string>(false, "User not found.", null);

            if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
                return new ApiResponse<string>(false, "Current password is incorrect.", null);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await _uow.Users.UpdateAsync(user);
            await _uow.SaveChangesAsync();

            return new ApiResponse<string>(true, "Password changed successfully.", null);
        }

        public async Task<ApiResponse<string>> LogoutAsync(int userId)
        {
            var user = await _uow.Users.GetByIdAsync(userId);
            if (user == null) return new ApiResponse<string>(false, "User not found.", null);

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await _uow.Users.UpdateAsync(user);
            await _uow.SaveChangesAsync();

            return new ApiResponse<string>(true, "Logged out successfully.", null);
        }

        private UserResponse MapUserResponse(User user, string companyName) => new(
            user.Id, user.FirstName, user.LastName, user.Email,
            user.Role.ToString(), user.CompanyId, companyName,
            user.IsActive, user.CreatedAt
        );

        public async Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest request)
        {
           var existingUser = await _uow.Users.GetByEmailAsync(request.Email);
            if (existingUser != null)
                return new ApiResponse<AuthResponse>(false, "Email already registered.", null);

            // Create company first
            var company = new Company
            {
                Name = request.CompanyName,
                Email = request.CompanyEmail,
                Phone = request.Phone,
                Address = request.Address,
                Country = request.Country,
                TaxNumber = request.TaxNumber
            };
            await _uow.Companies.AddAsync(company);
            await _uow.SaveChangesAsync();

            // Create admin user
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = UserRole.Admin,
                CompanyId = company.Id
            };
            await _uow.Users.AddAsync(user);

            // Generate tokens
            var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email, user.Role.ToString());
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _uow.SaveChangesAsync();

            return new ApiResponse<AuthResponse>(true, "Registration successful.", new AuthResponse(
                accessToken, refreshToken, DateTime.UtcNow.AddHours(1),
                MapUserResponse(user, company.Name)
            ));
        }

        public async Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request)
        {
             var user = await _uow.Users.GetByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return new ApiResponse<AuthResponse>(false, "Invalid email or password.", null);

            if (!user.IsActive)
                return new ApiResponse<AuthResponse>(false, "Account is deactivated.", null);

            var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email, user.Role.ToString());
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            user.UpdatedAt = DateTime.UtcNow;

            await _uow.Users.UpdateAsync(user);
            await _uow.SaveChangesAsync();

            return new ApiResponse<AuthResponse>(true, "Login successful.", new AuthResponse(
                accessToken, refreshToken, DateTime.UtcNow.AddHours(1),
                MapUserResponse(user, user.Company?.Name ?? "")
            ));
        }
            // TODO: NEED TO BE FIXED THE LOGIC AND THE MODEL
        public Task<ApiResponse<AuthResponse>> RegisterAsync(Microsoft.AspNetCore.Identity.Data.RegisterRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<AuthResponse>> LoginAsync(Microsoft.AspNetCore.Identity.Data.LoginRequest request)
        {
            throw new NotImplementedException();
        }
    }
}