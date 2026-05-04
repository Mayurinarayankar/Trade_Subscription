using WebApplication1.enums;

namespace WebApplication1.Models
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User;
        public bool IsActive { get; set; } = true;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        public string FullName => $"{FirstName} {LastName}";
    }
}