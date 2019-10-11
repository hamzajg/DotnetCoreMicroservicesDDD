using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Customer.App.Domain;
using Customer.App.Commands;
using Customer.App.UseCases;
using Services.Common.Commands;

namespace Customer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IRegisterCustomerUseCase _useCase;

        public CustomerController(ILogger<CustomerController> logger, IRegisterCustomerUseCase useCase)
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
