using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Order.Application.Commands;
using Order.Application.UseCases;

namespace Order.WebApi.Controllers
{
    /// <summary>
    /// The Order write endpoint. Depends on the CreateOrderUseCase abstraction
    /// (never the implementation) and echoes the accepted command, mirroring
    /// ProductsController and CustomerController. A saga failure propagates as
    /// OrderSagaFailedException, so the caller sees an error rather than a 202.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly CreateOrderUseCase _useCase;

        public OrdersController(ILogger<OrdersController> logger, CreateOrderUseCase useCase)
        {
            _logger = logger;
            _useCase = useCase;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateOrderCommand createOrder)
        {
            await _useCase.HandleAsync(createOrder);
            return Accepted(createOrder);
        }
    }
}
