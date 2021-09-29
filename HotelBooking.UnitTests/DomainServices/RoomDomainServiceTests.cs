using System.Collections.Generic;
using System.Linq;
using HotelBooking.Application.Common.Facade;
using HotelBooking.Application.Rooms;
using HotelBooking.Core;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests.DomainServices
{
    public class RoomDomainServiceTests
    {
        private Mock<IRepository<Room>> _mockRoomRepo;
        private RoomsDomainService _roomService;

        public RoomDomainServiceTests()
        {
            _mockRoomRepo = new Mock<IRepository<Room>>();
            _roomService = new RoomsDomainService(_mockRoomRepo.Object);
        }

        [Fact]
        public void Get_GetAll_AllRoomsReturned()
        {
            // Arrange
            _mockRoomRepo.Setup(r => r.GetAll()).Returns(new List<Room>
            {
                new Room{Id = 1, Description = "A"},
                new Room{ Id = 2, Description = "B"}
            });
            
            // Act
            var rooms = _roomService.GetAll();

            // Assert
            Assert.True(rooms.Count() > 0);
            _mockRoomRepo.Verify(r => r.GetAll(),Times.Once);
            _mockRoomRepo.VerifyNoOtherCalls();
        }
    }
}