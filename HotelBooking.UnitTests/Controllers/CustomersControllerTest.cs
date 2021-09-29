using HotelBooking.Application.Customers.Facade;
using HotelBooking.Core;
using HotelBooking.WebApi.Controllers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HotelBooking.UnitTests.Controllers
{
    public class CustomersControllerTest
    {
        private CustomersController _customerController;
        private Mock<ICustomerDomainService> _customerService;
        public CustomersControllerTest()
        {
            _customerService = new Mock<ICustomerDomainService>();
            _customerController = new CustomersController(_customerService.Object);
        }

        [Fact]
        public void GetAll_Happy_List()
        {
            //Assign
            var customers = new List<Customer>() 
            {
                new Customer() { Email="test1@mail.com", Name="Test_Name_1", Id=1 },
                new Customer() { Email="test2@mail.com", Name="Test_Name_2", Id=2 }
            };

            _customerService.Setup(s => s.GetAll()).Returns(customers);

            //Act
            var result = _customerController.Get();

            //Assert
            Assert.True(customers.Count() == result.Count());
            _customerService.Verify(s => s.GetAll(), Times.Once);
            _customerService.VerifyNoOtherCalls();
        }
    }
}
