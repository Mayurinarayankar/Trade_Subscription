using WebApplication1.Interfaces.Common;
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        Task<IEnumerable<Invoice>> GetByCompanyAsync(int companyId);
        Task<IEnumerable<Invoice>> GetByTradeAsync(int tradeId);
        Task<IEnumerable<Invoice>> GetBySubscriptionAsync(int subscriptionId);
        Task<string> GenerateInvoiceNumberAsync();
        Task<decimal> GetTotalRevenueAsync(int companyId);
    }
}