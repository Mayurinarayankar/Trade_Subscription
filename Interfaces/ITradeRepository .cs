using WebApplication1.Models;
using WebApplication1.Interfaces.Common;

namespace WebApplication1.Interfaces
{
    public interface ITradeRepository : IRepository<Trade>
    {
        Task<IEnumerable<Trade>> GetByCompanyAsync(int companyId);
        Task<Trade?> GetByTradeNumberAsync(string tradeNumber);
        Task<IEnumerable<Trade>> GetByStatusAsync(int companyId, string status);
        Task<string> GenerateTradeNumberAsync();
    }
}