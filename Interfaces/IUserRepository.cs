using WebApplication1.Interfaces.Common;
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetByCompanyAsync(int companyId);
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
    }
}