using WebApplication1.Models;

namespace WebApplication1.Interfaces.Common
{
    public interface IUnitOfWork : IDisposable
    {
        ICompanyRepository Companies { get; }
        IUserRepository Users { get; }
        ITradeRepository Trades { get; }
        ISubscriptionRepository Subscriptions { get; }
        IInvoiceRepository Invoices { get; }
        IIncotermRepository Incoterms { get; }
        ISubscriptionPlanRepository SubscriptionPlans { get; }
        Task<int> SaveChangesAsync();
    }

    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}