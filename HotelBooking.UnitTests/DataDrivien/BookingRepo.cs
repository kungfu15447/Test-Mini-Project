using System;
using HotelBooking.Core;
using HotelBooking.Infrastructure.Repositories;
using HotelBooking.IntegrationTests.Fixtures;
using Xunit;

namespace HotelBooking.UnitTests.DataDrivien
{
    public class BookingRepo:  IClassFixture<DatabaseFixture>
    {
        // This test class uses a separate Sqlite in-memory database. While the
        // .NET Core built-in in-memory database is not a relational database,
        // Sqlite in-memory database is. This means that an exception is thrown,
        // if a database constraint is violated, and this is a desirable behavior
        // when testing.

        BookingRepository bookingRepos;

        public BookingRepo(DatabaseFixture dbFixture)
        {
            var ctx = dbFixture.Context;
            // Create repositories and BookingManager
            bookingRepos = new BookingRepository(ctx);
        }

        [Fact]
        public void GetById_RoomFound_RoomWithSameId()
        {
            // Arrange
            var id = 1;
            
            // Act
            var booking = bookingRepos.Get(id);
            
            // Assert
            Assert.Equal(id, booking.Id);
        }
        
        [Fact]
        public void FindAvailableRoom_RoomNotAvailable_RoomIdIsMinusOne()
        {
            // Arrange
            var id = int.MaxValue;
            
            // Act
            Action act = () => bookingRepos.Get(id);
            
            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }
    }
}