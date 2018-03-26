using System.Threading;
using System.Threading.Tasks;

namespace OrderWebAPI.Repositories
{
    public interface IUnitOfWork
    {
         Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}