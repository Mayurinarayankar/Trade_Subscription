using WebApplication1.Interfaces.Common;
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface ISubscriptionPlanRepository : IRepository<SubscriptionPlanConfig>
    {
        Task<IEnumerable<SubscriptionPlanConfig>> GetActiveAsync();
        Task<SubscriptionPlanConfig?> GetByPlanTypeAsync(int planType);
    }
}