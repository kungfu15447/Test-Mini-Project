using System;
using HotelBooking.Application.Bookings;
using HotelBooking.Core;
using HotelBooking.Infrastructure;
using HotelBooking.Infrastructure.Repositories;
using HotelBooking.IntegrationTests.Fixtures;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HotelBooking.IntegrationTests
{
    public class BookingManagerTests:  IClassFixture<DatabaseFixture>
    {
        // This test class uses a separate Sqlite in-memory database. While the
        // .NET Core built-in in-memory database is not a relational database,
        // Sqlite in-memory database is. This means that an exception is thrown,
        // if a database constraint is violated, and this is a desirable behavior
        // when testing.

        BookingDomainService bookingDomainService;

        public BookingManagerTests(DatabaseFixture dbFixture)
        {
            var ctx = dbFixture.Context;
            // Create repositories and BookingManager
            var bookingRepos = new BookingRepository(ctx);
            var roomRepos = new RoomRepository(ctx);
            var dateTime = new DateTimeService();
            bookingDomainService = new BookingDomainService(bookingRepos, roomRepos, dateTime);
        }

        [Fact]
        public void FindAvailableRoom_RoomNotAvailable_RoomIdIsMinusOne()
        {
            // Act
            var roomId = bookingDomainService.FindAvailableRoom(DateTime.Today.AddDays(8), DateTime.Today.AddDays(8));
            // Assert
            Assert.Equal(-1, roomId);
        }
    }
}
