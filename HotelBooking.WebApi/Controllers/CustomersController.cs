using System;
using System.Collections.Generic;
using HotelBooking.Application.Customers.Facade;
using HotelBooking.Core;
using Microsoft.AspNetCore.Mvc;


namespace HotelBooking.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : Controller
    {
        private readonly ICustomerDomainService _customerService;

        public CustomersController(ICustomerDomainService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/customers
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return _customerService.GetAll();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var customer = _customerService.Get(id);
                return new OkObjectResult(customer);
            }catch(ArgumentException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

    }
}
