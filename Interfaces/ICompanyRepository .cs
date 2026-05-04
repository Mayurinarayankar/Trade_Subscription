using WebApplication1.Interfaces.Common;
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<Company?> GetByEmailAsync(string email);
        Task<Company?> GetWithSubscriptionsAsync(int id);
    }

}