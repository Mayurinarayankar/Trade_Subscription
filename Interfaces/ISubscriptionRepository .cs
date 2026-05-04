using WebApplication1.Interfaces.Common;
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface ISubscriptionRepository : IRepository<Subscription>
    {
        Task<Subscription?> GetActiveSubscriptionAsync(int companyId);
        Task<IEnumerable<Subscription>> GetByCompanyAsync(int companyId);
        Task<IEnumerable<Subscription>> GetExpiringSubscriptionsAsync(int daysAhead);
    }
}