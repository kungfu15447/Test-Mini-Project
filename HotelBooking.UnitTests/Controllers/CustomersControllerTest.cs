using HotelBooking.Application.Customers;
using HotelBooking.WebApi.Controllers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.UnitTests.Controllers
{
    public class CustomersControllerTest
    {
        private CustomersController _customerController;
        private Mock<CustomerDomainService> _customerService;
        public CustomersControllerTest()
        {
            _customerService = new Mock<CustomerDomainService>();
            _customerController = new CustomersController(_customerService.Object);
        }

        public void 
    }
}
