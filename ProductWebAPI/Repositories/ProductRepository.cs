using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductWebAPI.Contexts;
using ProductWebAPI.Models;

namespace ProductWebAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;

        public IUnitOfWork UnitOfWork 
        {
            get
            {
                return _context;
            }
        }

        public ProductRepository(ProductContext context)
        {
            _context = context;
        }

        public Product Add(Product product)
        {
            return _context.Products.Add(product).Entity;
        }

        public IEnumerable<Product> Get()
        {
            return _context.Products.ToList();
        }

        public Product GetProduct(int id)
        {
            var product = _context.Products
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (product == null)
            {
                throw new KeyNotFoundException();
            }

            return product;
        }

        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
        }
    }
}