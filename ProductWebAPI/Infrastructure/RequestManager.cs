using System;
using System.Threading.Tasks;
using ProductWebAPI.Contexts;

namespace ProductWebAPI.Infrastructure
{
    public class RequestManager : IRequestManager
    {
        private readonly ProductContext _context;

        public RequestManager(ProductContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistAsync(Guid id)
        {
            var request = await _context
                .FindAsync<ClientRequest>(id);

            return request != null;
        }

        public async Task CreateRequestForCommandAsync<T>(Guid id)
        {
            var exists = await ExistAsync(id);

            var request = exists ?
                throw new Exception($"Request with {id} already exists") :
                new ClientRequest()
                {
                    Id = id,
                    Name = typeof(T).Name,
                    Time = DateTime.Now
                };

            _context.Add(request);

            await _context.SaveChangesAsync();
        }
    }
}