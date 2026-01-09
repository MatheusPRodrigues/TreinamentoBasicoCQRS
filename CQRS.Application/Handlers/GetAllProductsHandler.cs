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
    public class GetAllProductsHandler
    {
        private readonly IMongoCollection<Product> _collection;

        public GetAllProductsHandler(ReadContext context)
        {
            _collection = context.GetDatabase().GetCollection<Product>("Products");
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
