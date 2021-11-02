using System;
using System.Collections.Generic;
using HotelBooking.Application.Bookings;
using HotelBooking.Application.Bookings.Facade;
using HotelBooking.Application.Common.Facade;
using HotelBooking.Core;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingDomainService bookingDomainService;
        private Mock<IRepository<Booking>> bookRepoMock;
        private Mock<IRepository<Room>> roomRepoMock;
        private Mock<IDateTimeService> dateTimeMock;
        private readonly DateTime today;

        public BookingManagerTests()
        {
            today = new DateTime(2021, 11, 1);
            
            bookRepoMock = new Mock<IRepository<Booking>>();
            roomRepoMock = new Mock<IRepository<Room>>();
            dateTimeMock = new Mock<IDateTimeService>();
            
            dateTimeMock.Setup(s => s.Today).Returns(today);
            
            bookingDomainService = new BookingDomainService(bookRepoMock.Object, roomRepoMock.Object, dateTimeMock.Object);
        }

        #region New
        
        [Fact]
        public void CreateBooking_StartDateBeforeToday_ThrowsArgumentException()
        {
            // Arrange
            var startBooking = today.AddDays(-1);
            var endBooking = today.AddDays(1);
            var booking = new Booking {StartDate = startBooking, EndDate = endBooking};

            // Act
            Action act = () => bookingDomainService.CreateBooking(booking);
            
            // Assert
            Assert.Throws<ArgumentException>(act);
        }
        
        [Fact]
        public void CreateBooking_StartDateAfterEndDate_ThrowsArgumentException()
        {
            // Arrange
            var startBooking = today.AddDays(1);
            var endBooking = today.AddDays(-1);
            var booking = new Booking {StartDate = startBooking, EndDate = endBooking};

            // Act
            Action act = () => bookingDomainService.CreateBooking(booking);
            
            // Assert
            Assert.Throws<ArgumentException>(act);
        }
        
        [Theory, MemberData(nameof(BorderValueData))]
        public void CreateBooking_BorderValues_CreatesOrNot(DateTime bookingStart, DateTime bookingEnd, bool result)
        {
            // Arrange
            SetUpForBorderValueTest();

            // Act
            bool bookingCreated = bookingDomainService.CreateBooking(new Booking { StartDate = bookingStart, EndDate = bookingEnd});
         
            // Assert
            Assert.Equal(result, bookingCreated);
        }
        
        public static readonly object[][] BorderValueData =
        {
            new object[] { new DateTime(2021, 11, 9), new DateTime(2021, 11, 11), false},
            new object[] { new DateTime(2021, 11, 11), new DateTime(2021, 11, 14), true},
            new object[] { new DateTime(2021, 11, 11), new DateTime(2021, 11, 15), false},
        };

        private void SetUpForBorderValueTest()
        {
            DateTime availableRoomPeriodStart1 = today.AddDays(4);
            DateTime availableRoomPeriodEnd1 = today.AddDays(9);
            DateTime availableRoomPeriodStart2 = today.AddDays(14);
            DateTime availableRoomPeriodEnd2 = today.AddDays(19);

            List<Booking> bookings = new List<Booking>
            {
                new Booking
                {
                    Id = 1, StartDate = availableRoomPeriodStart1, EndDate = availableRoomPeriodEnd1, IsActive = true,
                    CustomerId = 1, RoomId = 1
                },
                new Booking
                {
                    Id = 2, StartDate = availableRoomPeriodStart1, EndDate = availableRoomPeriodEnd1, IsActive = true,
                    CustomerId = 2, RoomId = 2
                },
                new Booking
                {
                    Id = 1, StartDate = availableRoomPeriodStart2, EndDate = availableRoomPeriodEnd2, IsActive = true,
                    CustomerId = 1, RoomId = 1
                },
                new Booking
                {
                    Id = 2, StartDate = availableRoomPeriodStart2, EndDate = availableRoomPeriodEnd2, IsActive = true,
                    CustomerId = 2, RoomId = 2
                },
            };
            List<Room> rooms = new List<Room>
            {
                new Room {Id = 1, Description = "A"},
                new Room {Id = 2, Description = "B"},
            };

            bookRepoMock.Setup(r => r.GetAll()).Returns(bookings);
            roomRepoMock.Setup(r => r.GetAll()).Returns(rooms);
        }

        #endregion


        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            // Arrange
            DateTime date = DateTime.Today.AddDays(-10);
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
            bookRepoMock.Verify(b => b.Add(booking), Times.Once);
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
