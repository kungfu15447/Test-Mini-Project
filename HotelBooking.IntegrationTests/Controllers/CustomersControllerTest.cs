using HotelBooking.Application.Customers;
using HotelBooking.Core;
using HotelBooking.Infrastructure;
using HotelBooking.Infrastructure.Repositories;
using HotelBooking.IntegrationTests.Fixtures;
using HotelBooking.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HotelBooking.IntegrationTests.Controllers
{
    public class CustomersControllerTest : IClassFixture<DatabaseFixture>
    {
        private CustomersController _controller;
        private HotelBookingContext _ctx;
        public CustomersControllerTest(DatabaseFixture dbFixture)
        {
            _ctx = dbFixture.Context;

            var repo = new CustomerRepository(_ctx);
            var service = new CustomerDomainService(repo);
            _controller = new CustomersController(service);
        }


        [Fact]
        public void GetAll_Happy_List()
        {
            //Arrange
            var dbCustomers = _ctx.Customer.ToList();

            //Act
            var result = _controller.Get().ToList();

            //Assert
            Assert.True(result is not null);
            Assert.Equal(result, dbCustomers);
        }

        [Fact]
        public void Get_CustomerDoesNotExist_ReturnsBadRequestObjectResult()
        {
            //Arrange
            var id = -1;

            //Act
            var result = _controller.Get(id);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Get_CustomerExists_ReturnsOkObjectResult()
        {
            //Arrange
            var customer = _ctx.Customer.FirstOrDefault();
            var id = customer.Id;

            //Act
            var result = _controller.Get(id);

            //Assert
            var correctResult = Assert.IsType<OkObjectResult>(result);
            var customerFromResult = Assert.IsType<Customer>(correctResult.Value);
            Assert.Equal(customer, customerFromResult);
        }



    }
}
