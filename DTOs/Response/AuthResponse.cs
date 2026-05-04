namespace WebApplication1.DTOs.Response
{
    public record AuthResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    UserResponse User
);

public record UserResponse(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string Role,
    int CompanyId,
    string CompanyName,
    bool IsActive,
    DateTime CreatedAt
);

}