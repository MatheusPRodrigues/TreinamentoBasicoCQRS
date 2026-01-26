using CQRS.Domain.DTOs;
using CQRS.Domain.Entities;
using CQRS.Infraestructure.Context;
using MongoDB.Driver;

namespace CQRS.Application.Handlers
{
    public class GetAllProductsHandler
    {
        private readonly IMongoCollection<Product> _collection;

        public GetAllProductsHandler(IAbstractFactory<IMongoDatabase> context)
        {
            _collection = context.CreateConnection().GetCollection<Product>("Products");
        }

        public async Task<IEnumerable<ProductDTO?>> HandleAsync()
        {
            var products = await _collection.FindAsync<Product>(p => true).Result.ToListAsync();
            var response = products
                .Select(p => new ProductDTO
                {
                    Name = p.Name,
                    Price = p.Price
                })
                .ToList();
            return response;
        }
    }
}
