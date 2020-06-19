using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Customer.App.Commands;
using Customer.App.UseCases;

namespace Customer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly RegisterCustomerUseCase _useCase;

        public CustomerController(ILogger<CustomerController> logger, RegisterCustomerUseCase useCase)
        {
            _logger = logger;
            _useCase = useCase;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterCustomer registerCustomer) 
        {
            registerCustomer.Id = Guid.NewGuid();
            await _useCase.HandleAsync(registerCustomer);
            return Accepted(registerCustomer);
        }
    }
}
