using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Product.Application.Commands;
using Product.Application.UseCases;

namespace Product.WebApi.Controllers
{
    /// <summary>
    /// The Product write endpoint. Depends on the CreateProductUseCase
    /// abstraction (never the implementation) and echoes the accepted command,
    /// mirroring CustomerController so both services expose the same shape.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly CreateProductUseCase _useCase;

        public ProductsController(ILogger<ProductsController> logger, CreateProductUseCase useCase)
        {
            _logger = logger;
            _useCase = useCase;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProductCommand createProduct)
        {
            await _useCase.HandleAsync(createProduct);
            return Accepted(createProduct);
        }
    }
}
