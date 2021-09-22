using HotelBooking.Application.Common.Facade;
using HotelBooking.Application.Customers;
using HotelBooking.Application.Customers.Facade;
using HotelBooking.Core;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class CustomerServiceTests
    {
        private Mock<IRepository<Customer>> _mockCustomerRepo;
        private ICustomerDomainService _customerService;
        public CustomerServiceTests()
        {
            _mockCustomerRepo = new Mock<IRepository<Customer>>();
            _customerService = new CustomerDomainService(_mockCustomerRepo.Object);
        }

        [Fact]
        public void Get_GetCustomerById_CustomerReturned()
        {
            //Arrange
            var customerId = 1;
            _mockCustomerRepo.Setup(r => r.Get(customerId)).Returns(new Customer() { });

            //Act
            var customer = _customerService.Get(customerId);

            //Assert
            Assert.False(customer is null);
            _mockCustomerRepo.Verify(r => r.Get(customerId), Times.Once);
            _mockCustomerRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Get_CustomerIsNull_ArgumentExceptionThrown()
        {
            //Arrange
            var customerId = 1;
            _mockCustomerRepo.Setup(r => r.Get(customerId)).Returns<Customer>(null);

            //Act
            Action act = () => _customerService.Get(customerId);

            //Assert
            Assert.Throws<ArgumentException>(act);
            _mockCustomerRepo.Verify(r => r.Get(customerId), Times.Once);
            _mockCustomerRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Add_CustomerIsAdded_MethodIsCalled()
        {
            //Arrange
            var customer = new Customer();
            _mockCustomerRepo.Setup(r => r.Add(customer));

            //Act
            _customerService.Add(customer);

            //Assert
            _mockCustomerRepo.Verify(r => r.Add(customer), Times.Once);
            _mockCustomerRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Edit_CustomerIsUpdated_MethodIsCalled()
        {
            //Arrange
            var customer = new Customer();
            _mockCustomerRepo.Setup(r => r.Edit(customer));

            //Act
            _customerService.Update(customer);

            //Assert
            _mockCustomerRepo.Verify(r => r.Edit(customer), Times.Once);
            _mockCustomerRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Delete_CustomerIsDeleted_MethodIsCalled()
        {
            //Arrange
            var customerId = 1;
            _mockCustomerRepo.Setup(r => r.Remove(customerId));

            //Act
            _customerService.Remove(customerId);

            //Assert
            _mockCustomerRepo.Verify(r => r.Remove(customerId), Times.Once);
            _mockCustomerRepo.VerifyNoOtherCalls();
        }
    }
}
