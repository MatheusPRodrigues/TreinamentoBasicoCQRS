using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.DTOs
{
    public class ProductDTO
    {
        public string Name { get; init; }
        public decimal Price { get; init; }
    }
}
