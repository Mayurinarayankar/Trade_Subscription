namespace WebApplication1.DTOs.Request
{
    public record RegisterRequest(
     string FirstName,
     string LastName,
     string Email,
     string Password,
     string CompanyName,
     string CompanyEmail,
     string Phone,
     string Address,
     string Country,
     string TaxNumber);
    public record LoginRequest(string Email, string Password);

    public record RefreshTokenRequest(string RefreshToken);

    public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
}