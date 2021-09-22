using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Application.Bookings.Facade;
using HotelBooking.Core;
using HotelBooking.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests.Controllers
{
    public class BookingControllerTest
    {
        private BookingsController controller;
        private Mock<IBookingDomainService> mockService;

        public BookingControllerTest()
        {
            mockService = new Mock<IBookingDomainService>();
            controller = new BookingsController(mockService.Object);
        }


        [Fact]
        public void GetAll_Happy_List()
        {
            // Arrange
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            List<Booking> bookings = new List<Booking>
            {
                new Booking {Id = 1, StartDate = start, EndDate = end, IsActive = true, CustomerId = 1, RoomId = 1},
                new Booking {Id = 2, StartDate = start, EndDate = end, IsActive = true, CustomerId = 2, RoomId = 2},
            };

            mockService.Setup(r => r.GetAll()).Returns(bookings);

            // Act
            var result = controller.Get();

            // Assert
            Assert.True(result.Count() == bookings.Count);
            mockService.Verify(r => r.GetAll(), Times.Once);
            mockService.VerifyNoOtherCalls();
        }

        [Fact]
        public void Get_Happy_ObjectResultWithBooking()
        {
            // Arrange
            var id = 1;
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            List<Booking> bookings = new List<Booking>
            {
                new Booking {Id = 1, StartDate = start, EndDate = end, IsActive = true, CustomerId = 1, RoomId = 1},
                new Booking {Id = 2, StartDate = start, EndDate = end, IsActive = true, CustomerId = 2, RoomId = 2},
            };

            mockService.Setup(r => r.Get(id)).Returns(bookings[0]);

            // Act
            var result = controller.Get(id);

            // Assert
            var oResult = Assert.IsType<ObjectResult>(result);
            Assert.IsType<Booking>(oResult.Value);
            mockService.Verify(r => r.Get(id), Times.Once);
            mockService.VerifyNoOtherCalls();
        }

        [Fact]
        public void Get_DomainReturnNull_NotFound()
        {
            // Arrange
            var id = 1;
            mockService.Setup(r => r.Get(id)).Returns((Booking) null);

            // Act
            var result = controller.Get(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            mockService.Verify(r => r.Get(id), Times.Once);
            mockService.VerifyNoOtherCalls();
        }

        [Fact]
        public void Post_ArgumentNull_BadRequest()
        {
            // Arrange
            Booking booking = null;
            mockService.Setup(r => r.CreateBooking(booking)).Returns(false);

            // Act
            var result = controller.Post(booking);

            // Assert
            Assert.IsType<BadRequestResult>(result);
            mockService.Verify(r => r.CreateBooking(booking), Times.Never);
            mockService.VerifyNoOtherCalls();
        }

        [Fact]
        public void Post_Happy_CreatedAtRoute()
        {
            // Arrange
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            Booking booking = new Booking
            {
                StartDate = start,
                EndDate = end,
                IsActive = true,
                CustomerId = 1,
                RoomId = 1
            };
            mockService.Setup(r => r.CreateBooking(booking)).Returns(true);

            // Act
            var result = controller.Post(booking);

            // Assert
            var createdAtRoute = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal("GetBookings", createdAtRoute.RouteName);
            mockService.Verify(r => r.CreateBooking(booking), Times.Once);
            mockService.VerifyNoOtherCalls();
        }

        [Fact]
        public void Post_CreateBookingReturnNull_Conflict()
        {
            // Arrange
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            Booking booking = new Booking
            {
                StartDate = start,
                EndDate = end,
                IsActive = true,
                CustomerId = 1,
                RoomId = 1
            };
            mockService.Setup(r => r.CreateBooking(booking)).Returns(false);

            // Act
            var result = controller.Post(booking);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
            mockService.Verify(r => r.CreateBooking(booking), Times.Once);
            mockService.VerifyNoOtherCalls();
        }

        [Fact]
        public void Put_ArgumentBookingNull_BadRequest()
        {
            // Arrange
            int id = 2;
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            Booking booking = new Booking
            {
                Id= 1,
                StartDate = start,
                EndDate = end,
                IsActive = true,
                CustomerId = 1,
                RoomId = 1
            };
            mockService.Setup(r => r.Get(id)).Returns(booking);
            mockService.Setup(r => r.Edit(booking));

            // Act
            var result = controller.Put(id, null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
            mockService.Verify(r => r.Get(id), Times.Never);
            mockService.Verify(r => r.Edit(booking), Times.Never);
            mockService.VerifyNoOtherCalls();
        }

        [Fact]
        public void Put_ArgumentIdNotBookingId_BadRequest()
        {
            int id = 2;
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            Booking booking = new Booking
            {
                Id= 1,
                StartDate = start,
                EndDate = end,
                IsActive = true,
                CustomerId = 1,
                RoomId = 1
            };
            mockService.Setup(r => r.Get(id)).Returns(booking);
            mockService.Setup(r => r.Edit(booking));

            // Act
            var result = controller.Put(id, booking);

            // Assert
            Assert.IsType<BadRequestResult>(result);
            mockService.Verify(r => r.Get(id), Times.Never);
            mockService.Verify(r => r.Edit(booking), Times.Never);
            mockService.VerifyNoOtherCalls();
        }

        [Fact]
        public void Put_GetReturnNull_NotFound()
        {
            // Arrange
            int id = 1;
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            Booking booking = new Booking
            {
                Id= 1,
                StartDate = start,
                EndDate = end,
                IsActive = true,
                CustomerId = 1,
                RoomId = 1
            };
            mockService.Setup(r => r.Get(id)).Returns((Booking) null);
            mockService.Setup(r => r.Edit(booking));

            // Act
            var result = controller.Put(id, booking);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            mockService.Verify(r => r.Get(id), Times.Once);
            mockService.Verify(r => r.Edit(booking), Times.Never);
            mockService.VerifyNoOtherCalls();
        }

        [Fact]
        public void Put_Happy_NoContent()
        {
            // Arrange
            int id = 1;
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            Booking booking = new Booking
            {
                Id= id,
                StartDate = start,
                EndDate = end,
                IsActive = true,
                CustomerId = 1,
                RoomId = 1
            };
            mockService.Setup(r => r.Get(id)).Returns(booking);
            mockService.Setup(r => r.Edit(booking));

            // Act
            var result = controller.Put(id, booking);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mockService.Verify(r => r.Get(id), Times.Once);
            mockService.Verify(r => r.Edit(booking), Times.Once);
            mockService.VerifyNoOtherCalls();
        }

        [Fact]
        public void Delete_GetReturnNull_NotFound()
        {
            // Arrange
            int id = 1;
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            Booking booking = null;
            
            mockService.Setup(r => r.Get(id)).Returns(booking);
            mockService.Setup(r => r.Remove(id));

            // Act
            var result = controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            mockService.Verify(r => r.Get(id), Times.Once);
            mockService.Verify(r => r.Edit(booking), Times.Never);
            mockService.VerifyNoOtherCalls();
        }

        [Fact]
        public void Delete_Happy_NoContent()
        {
            // Arrange
            int id = 1;
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            Booking booking = new Booking
            {
                StartDate = start,
                EndDate = end,
                IsActive = true,
                CustomerId = 1,
                RoomId = 1
            };
            mockService.Setup(r => r.Get(id)).Returns(booking);
            mockService.Setup(r => r.Remove(id));

            // Act
            var result = controller.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mockService.Verify(r => r.Get(id), Times.Once);
            mockService.Verify(r => r.Remove(id), Times.Once);
            mockService.VerifyNoOtherCalls();
        }
    }
}