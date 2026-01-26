using CQRS.Domain.DTOs;
using CQRS.Domain.Entities;
using CQRS.Infraestructure.Context;
using MongoDB.Driver;

namespace CQRS.Application.Handlers
{
    public class GetProductByIdHandler
    {
        private readonly IMongoCollection<Product> _collection;

        public GetProductByIdHandler(IAbstractFactory<IMongoDatabase> context)
        {
            _collection = context.CreateConnection().GetCollection<Product>("Products");
        }

        public async Task<ProductDTO?> HandleAsync(int id)
        {
            var product = await _collection.FindAsync<Product>(p => p.Id == id).Result.FirstOrDefaultAsync();
            if (product is null)
                return null;

            return new ProductDTO
            {
                Name = product.Name,
                Price = product.Price,
            };
        }
    }
}
