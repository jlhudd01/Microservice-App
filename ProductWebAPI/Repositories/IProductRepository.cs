using System.Collections.Generic;
using ProductWebAPI.Models;

namespace ProductWebAPI.Repositories
{
    public interface IProductRepository
    {
         Product Add(Product product);
         IEnumerable<Product> Get();
         Product GetProduct(int id);
         void Update(Product product);
         void Delete(Product product);
         IUnitOfWork UnitOfWork { get; }
    }
}