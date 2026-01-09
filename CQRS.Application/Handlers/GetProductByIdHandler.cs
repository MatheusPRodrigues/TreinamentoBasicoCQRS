using CQRS.Domain.DTOs;
using CQRS.Domain.Entities;
using CQRS.Infraestructure.Context;
using Dapper;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Application.Handlers
{
    public class GetProductByIdHandler
    {
        private readonly IMongoCollection<Product> _collection;

        public GetProductByIdHandler(ReadContext context)
        {
            _collection = context.GetDatabase().GetCollection<Product>("Products");
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
