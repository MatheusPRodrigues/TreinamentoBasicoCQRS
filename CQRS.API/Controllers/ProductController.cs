using CQRS.Application.Commands;
using CQRS.Application.Handlers;
using CQRS.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CQRS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly CreateProductHandler _createProductHandler;
        private readonly GetProductByIdHandler _getProductByIdHandler;
        private readonly GetAllProductsHandler _getAllProductsHandler;

        public ProductController(
            CreateProductHandler createProductHandler,
            GetProductByIdHandler getProductByIdHandler,
            GetAllProductsHandler getAllProductsHandler
        )
        {
            _createProductHandler = createProductHandler;
            _getProductByIdHandler = getProductByIdHandler;
            _getAllProductsHandler = getAllProductsHandler;
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateProductAsync([FromBody] CreateProductCommand command)
        {
            var result = await _createProductHandler.HandleAsync(command);
            return StatusCode(StatusCodes.Status201Created, new { Id = result });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProductByIdAsync(int id)
        {
            var product = await _getProductByIdHandler.HandleAsync(id);
            if (product is null)
                return NotFound(new { Message = "Produto não encontrado!" });

            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProductsAsync()
        {
            var products = await _getAllProductsHandler.HandleAsync();
            
            return Ok(products);
        }
    }
}
