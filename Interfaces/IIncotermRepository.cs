using WebApplication1.Interfaces.Common;
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IIncotermRepository : IRepository<Incoterm>
    {
        Task<Incoterm?> GetByCodeAsync(string code);
        Task<IEnumerable<Incoterm>> GetActiveAsync();
    }
}