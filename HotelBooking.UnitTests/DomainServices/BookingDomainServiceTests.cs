using System;
using System.Collections.Generic;
using HotelBooking.Application.Bookings;
using HotelBooking.Application.Bookings.Facade;
using HotelBooking.Application.Common.Facade;
using HotelBooking.Core;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingDomainService bookingDomainService;
        private Mock<IRepository<Booking>> bookRepoMock;
        private Mock<IRepository<Room>> roomRepoMock;

        public BookingManagerTests()
        {
            bookRepoMock = new Mock<IRepository<Booking>>();
            roomRepoMock = new Mock<IRepository<Room>>();
            bookingDomainService = new BookingDomainService(bookRepoMock.Object, roomRepoMock.Object);
        }

        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            // Arrange
            DateTime date = DateTime.Today;
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            List<Booking> bookings = new List<Booking>
            {
                new Booking { Id=1, StartDate=start, EndDate=end, IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=start, EndDate=end, IsActive=true, CustomerId=2, RoomId=2 },
            };
            List<Room> rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
            };

            bookRepoMock.Setup(r => r.GetAll()).Returns(bookings);
            roomRepoMock.Setup(r => r.GetAll()).Returns(rooms);

            // Act
            Action act = () => bookingDomainService.FindAvailableRoom(date, date);

            // Assert
            Assert.Throws<ArgumentException>(act);
            bookRepoMock.Verify(r => r.GetAll(), Times.Never);
            roomRepoMock.Verify(r => r.GetAll(), Times.Never);
            bookRepoMock.VerifyNoOtherCalls();
            roomRepoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne()
        {
            // Arrange
            DateTime date = DateTime.Today.AddDays(1);
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            List<Booking> bookings = new List<Booking>
            {
                new Booking { Id=1, StartDate=start, EndDate=end, IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=start, EndDate=end, IsActive=true, CustomerId=2, RoomId=2 },
            };
            List<Room> rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
            };

            bookRepoMock.Setup(r => r.GetAll()).Returns(bookings);
            roomRepoMock.Setup(r => r.GetAll()).Returns(rooms);

            // Act
            int roomId = bookingDomainService.FindAvailableRoom(date, date);
            // Assert
            Assert.NotEqual(-1, roomId);
            bookRepoMock.Verify(r => r.GetAll(), Times.Once);
            roomRepoMock.Verify(r => r.GetAll(), Times.Once);
            bookRepoMock.VerifyNoOtherCalls();
            roomRepoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void FindAvailableRoom_RoomAvaiable_RoomIdIsMinusOne()
        {
            // Arrange
            DateTime date = DateTime.Today.AddDays(1);
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            List<Booking> bookings = new List<Booking>
            {
                new Booking { Id=1, StartDate=date, EndDate=end, IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=date, EndDate=end, IsActive=true, CustomerId=2, RoomId=2 },
            };
            List<Room> rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
            };

            bookRepoMock.Setup(r => r.GetAll()).Returns(bookings);
            roomRepoMock.Setup(r => r.GetAll()).Returns(rooms);

            // Act
            int roomId = bookingDomainService.FindAvailableRoom(date, date);

            //Assert
            Assert.Equal(roomId, -1);
            bookRepoMock.Verify(r => r.GetAll(), Times.Once);
            roomRepoMock.Verify(r => r.GetAll(), Times.Once);
            bookRepoMock.VerifyNoOtherCalls();
            roomRepoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void CreateBooking_BookingIsSuccessfull_ReturnsTrue()
        {
            //Arrange
            DateTime bookingStartDate = DateTime.Today.AddDays(1);
            DateTime bookingEndDate = DateTime.Today.AddDays(2);
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            List<Booking> bookings = new List<Booking>
            {
                new Booking { Id=1, StartDate=start, EndDate=end, IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=start, EndDate=end, IsActive=true, CustomerId=2, RoomId=2 },
            };
            List<Room> rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
            };

            bookRepoMock.Setup(r => r.GetAll()).Returns(bookings);
            roomRepoMock.Setup(r => r.GetAll()).Returns(rooms);

            var booking = new Booking() { StartDate=bookingStartDate, EndDate=bookingEndDate };

            //Act
            var result = bookingDomainService.CreateBooking(booking);

            //Assert
            Assert.True(result);
            bookRepoMock.Verify(r => r.Add(booking), Times.Once);
            bookRepoMock.Verify(r => r.GetAll(), Times.Once);
            bookRepoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void CreateBooking_BookingIsUnsuccessfull_ReturnsFalse()
        {
            // Arrange
            DateTime bookingStartDate = DateTime.Today.AddDays(1);
            DateTime bookingEndDate = DateTime.Today.AddDays(2);
            DateTime start = DateTime.Today.AddDays(1);
            DateTime end = DateTime.Today.AddDays(20);
            List<Booking> bookings = new List<Booking>
            {
                new Booking { Id=1, StartDate=start, EndDate=end, IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=start, EndDate=end, IsActive=true, CustomerId=2, RoomId=2 },
            };
            List<Room> rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
            };

            bookRepoMock.Setup(r => r.GetAll()).Returns(bookings);
            roomRepoMock.Setup(r => r.GetAll()).Returns(rooms);

            var booking = new Booking() { StartDate = bookingStartDate, EndDate = bookingEndDate };

            //Act
            var result = bookingDomainService.CreateBooking(booking);

            //Assert
            Assert.False(result);
            bookRepoMock.Verify(r => r.Add(booking), Times.Never);
            bookRepoMock.Verify(r => r.GetAll(), Times.Once);
            bookRepoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void CreateBooking_BookingIsNull_ThrowsArgumentExcecption()
        {
            //Arrange
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            List<Booking> bookings = new List<Booking>
            {
                new Booking { Id=1, StartDate=start, EndDate=end, IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=start, EndDate=end, IsActive=true, CustomerId=2, RoomId=2 },
            };
            List<Room> rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
            };

            bookRepoMock.Setup(r => r.GetAll()).Returns(bookings);
            roomRepoMock.Setup(r => r.GetAll()).Returns(rooms);

            Booking booking = null;

            //Act
            Action act = () => bookingDomainService.CreateBooking(booking);

            //Assert
            Assert.Throws<ArgumentException>(act);
            bookRepoMock.Verify(r => r.Add(booking), Times.Never);
            bookRepoMock.Verify(r => r.GetAll(), Times.Never);
            bookRepoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void GetFullyOccupiedDates_NoOccupiedDates_ReturnsEmptyList()
        {
            //Arrange
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            List<Booking> bookings = new List<Booking>
            {
                new Booking { Id=1, StartDate=start, EndDate=end, IsActive=false, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=start, EndDate=end, IsActive=true, CustomerId=2, RoomId=2 },
            };
            List<Room> rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
            };

            bookRepoMock.Setup(r => r.GetAll()).Returns(bookings);
            roomRepoMock.Setup(r => r.GetAll()).Returns(rooms);

            //Act
            var fullyOccupiedDates = bookingDomainService.GetFullyOccupiedDates(start, end);

            //Assert
            Assert.True(fullyOccupiedDates.Count == 0);
            bookRepoMock.Verify(r => r.GetAll(), Times.Once);
            roomRepoMock.Verify(r => r.GetAll(), Times.Once);
            bookRepoMock.VerifyNoOtherCalls();
            roomRepoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void GetFullyOccupiedDates_StartDateAfterEndDate_ThrowsArgumentException()
        {
            //Arrange
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            List<Booking> bookings = new List<Booking>
            {
                new Booking { Id=1, StartDate=start, EndDate=end, IsActive=false, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=start, EndDate=end, IsActive=true, CustomerId=2, RoomId=2 },
            };
            List<Room> rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
            };

            bookRepoMock.Setup(r => r.GetAll()).Returns(bookings);
            roomRepoMock.Setup(r => r.GetAll()).Returns(rooms);

            //Act
            Action act = () => bookingDomainService.GetFullyOccupiedDates(end, start);

            //Assert
            Assert.Throws<ArgumentException>(act);
            bookRepoMock.Verify(r => r.GetAll(), Times.Never);
            roomRepoMock.Verify(r => r.GetAll(), Times.Never);
            bookRepoMock.VerifyNoOtherCalls();
            roomRepoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void GetFullyOccupiedDates_OccupiedDates_ReturnsNotEmptyList()
        {
            //Arrange
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            List<Booking> bookings = new List<Booking>
            {
                new Booking { Id=1, StartDate=start, EndDate=end, IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=start, EndDate=end, IsActive=true, CustomerId=2, RoomId=2 },
            };
            List<Room> rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
            };

            bookRepoMock.Setup(r => r.GetAll()).Returns(bookings);
            roomRepoMock.Setup(r => r.GetAll()).Returns(rooms);

            //Act
            var fullyOccupiedDates = bookingDomainService.GetFullyOccupiedDates(start, end);

            //Assert
            Assert.True(fullyOccupiedDates.Count > 0);
            bookRepoMock.Verify(r => r.GetAll(), Times.Once);
            roomRepoMock.Verify(r => r.GetAll(), Times.Once);
            bookRepoMock.VerifyNoOtherCalls();
            roomRepoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void GetAll_MethodInvocation_CallsRepo()
        {
            // Arrange
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            List<Booking> bookings = new List<Booking>
            {
                new Booking { Id=1, StartDate=start, EndDate=end, IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=start, EndDate=end, IsActive=true, CustomerId=2, RoomId=2 },
            };
            bookRepoMock.Setup(r => r.GetAll()).Returns(bookings);
            
            // Act
            bookingDomainService.GetAll();

            // Assert
            bookRepoMock.Verify(r => r.GetAll(), Times.Once);
            bookRepoMock.VerifyNoOtherCalls();
        }
        
        [Fact]
        public void Get_MethodInvocation_CallsRepo()
        {
            // Arrange
            int id = 2;
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            List<Booking> bookings = new List<Booking>
            {
                new Booking { Id=1, StartDate=start, EndDate=end, IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=start, EndDate=end, IsActive=true, CustomerId=2, RoomId=2 },
            };
            bookRepoMock.Setup(r => r.Get(id)).Returns(bookings[1]);
            
            // Act
            bookingDomainService.Get(id);

            // Assert
            bookRepoMock.Verify(r => r.Get(id), Times.Once);
            bookRepoMock.VerifyNoOtherCalls();
        }
        
        [Fact]
        public void Edit_MethodInvocation_CallsRepo()
        {
            // Arrange
            int id = 2;
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);

            var book = new Booking {Id = 1, StartDate = start, EndDate = end, IsActive = true, CustomerId = 1, RoomId = 1};
            bookRepoMock.Setup(r => r.Edit(book));
            
            // Act
            bookingDomainService.Edit(book);

            // Assert
            bookRepoMock.Verify(r => r.Edit(book), Times.Once);
            bookRepoMock.VerifyNoOtherCalls();
        }
        
        [Fact]
        public void Delete_MethodInvocation_CallsRepo()
        {
            int id = 2;
            bookRepoMock.Setup(r => r.Remove(id));
            
            // Act
            bookingDomainService.Remove(id);

            // Assert
            bookRepoMock.Verify(r => r.Remove(id), Times.Once);
            bookRepoMock.VerifyNoOtherCalls();
        }
    }
}
