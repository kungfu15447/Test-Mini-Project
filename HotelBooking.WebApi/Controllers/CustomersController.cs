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

    }
}
