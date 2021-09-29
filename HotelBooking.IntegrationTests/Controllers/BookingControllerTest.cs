using System;
using System.Linq;
using HotelBooking.Application.Bookings;
using HotelBooking.Core;
using HotelBooking.Infrastructure.Repositories;
using HotelBooking.IntegrationTests.Fixtures;
using HotelBooking.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace HotelBooking.IntegrationTests.Controllers
{
    public class BookingControllerTest : IClassFixture<DatabaseFixture>
    {
        private BookingsController controller;

        public BookingControllerTest(DatabaseFixture dbFixture)
        {
            var ctx = dbFixture.Context;
            // Create repositories and BookingManager
            var bookingRepos = new BookingRepository(ctx);
            var roomRepos = new RoomRepository(ctx);
            var bookingDomainService = new BookingDomainService(bookingRepos, roomRepos);
            controller = new BookingsController(bookingDomainService);
        }

        [Fact]
        public void GetAll_Happy_List()
        {
            // Act
            var result = controller.Get();

            // Assert
            Assert.True(result is not null);
        }

        [Fact]
        public void Get_Happy_ObjectResultWithBooking()
        {
            // Arrange
            var id = 1;

            // Act
            var result = controller.Get(id);

            // Assert
            var oResult = Assert.IsType<ObjectResult>(result);
            var booking = Assert.IsType<Booking>(oResult.Value);
            Assert.True(booking.Id == id);
        }

        [Fact]
        public void Get_DomainReturnNull_NotFound()
        {
            // Arrange
            var id = Int32.MaxValue;

            // Act
            var result = controller.Get(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Post_Happy_CreatedAtRoute()
        {
            // Arrange
            DateTime start = DateTime.Today.AddDays(100);
            DateTime end = DateTime.Today.AddDays(107);
            Booking booking = new Booking
            {
                StartDate = start,
                EndDate = end,
                IsActive = true,
                CustomerId = 1,
                RoomId = 1
            };

            // Act
            var result = controller.Post(booking);

            // Assert
            var createdAtRoute = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal("GetBookings", createdAtRoute.RouteName);
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

            // Act
            var result = controller.Post(booking);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public void Put_GetReturnNull_NotFound()
        {
            // Arrange
            var id = Int32.MaxValue;
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            Booking booking = new Booking
            {
                Id = id,
                StartDate = start,
                EndDate = end,
                IsActive = true,
                CustomerId = 1,
                RoomId = 1
            };

            // Act
            var result = controller.Put(id, booking);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Put_Happy_NoContent()
        {
            // Arrange
            var entry = controller.Get();
            var booking = entry.ToList()[0];
            booking.IsActive = !booking.IsActive;
            var expected = booking.IsActive;

            // Act
            var result = controller.Put(booking.Id, booking);

            // Assert
            var afterUpdate = (Booking) ((ObjectResult) controller.Get(booking.Id)).Value;
            
            Assert.IsType<NoContentResult>(result);
            
            Assert.True(expected == afterUpdate.IsActive);
        }

        [Fact]
        public void Delete_GetReturnNull_NotFound()
        {
            // Arrange
            var id = Int32.MaxValue;
            var entriesBefore = controller.Get().Count();

            // Act
            var result = controller.Delete(id);

            // Assert
            var entriesAfter = controller.Get().Count();

            Assert.IsType<NotFoundResult>(result);
            Assert.True(entriesAfter == entriesBefore);
        }

        [Fact]
        public void Delete_Happy_NoContent()
        {
            // Arrange
            var id = 1;
            var entriesBefore = controller.Get().Count();

            // Act
            var result = controller.Delete(id);

            // Assert
            var entriesAfter = controller.Get().Count();

            Assert.IsType<NoContentResult>(result);
            Assert.True(entriesAfter == entriesBefore - 1);
        }
    }
}