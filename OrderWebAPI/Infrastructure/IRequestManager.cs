using System;
using System.Threading.Tasks;

namespace OrderWebAPI.Infrastructure
{
    public interface IRequestManager
    {
         Task<bool> ExistAsync(Guid id);

         Task CreateRequestForCommandAsync<T>(Guid id);
    }
}