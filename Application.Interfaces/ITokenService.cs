namespace WebApplication1.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(int userId, string email, string role);
        string GenerateRefreshToken();
        int? ValidateToken(string token);
    }
}