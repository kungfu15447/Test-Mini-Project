using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Application.Common.Facade;
using HotelBooking.Application.Rooms.Facade;
using HotelBooking.Core;
using HotelBooking.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests.Controllers
{
    public class RoomsControllerTests
    {
        private RoomsController controller;
        private Mock<IRoomsDomainService> mockService;

        public RoomsControllerTests()
        {
            mockService = new Mock<IRoomsDomainService>();
            controller = new RoomsController(mockService.Object);
        }

        [Fact]
        public void GetAll_Happy_ArrayOfRooms() 
        { 
            var rooms = new List<Room>
            {
                new Room {Id = 1, Description = "A"},
                new Room {Id = 2, Description = "B"},
            };

            mockService.Setup(s => s.GetAll()).Returns(rooms);

            // Act
            var result = controller.Get();
            var noOfRooms = result.Count();

            // Assert
            Assert.Equal(2, noOfRooms);
            mockService.Verify(s => s.GetAll(), Times.Once);
        }

        [Fact]
        public void GetById_RoomExists_ReturnsIActionResultWithRoom()
        {
            // Arrange 
            var id = 2;

            Room outputRoom = new Room { Id = 2, Description = "B" };

            mockService.Setup(s => s.Get(id)).Returns(outputRoom);

            // Act
            var result = controller.Get(id);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Room room = Assert.IsType<Room>(objectResult.Value);

            Assert.True(room.Id == id);
            Assert.InRange<int>(room.Id, 1, 2);
        }

        [Fact]
        public void Delete_WhenIdIsLargerThanZero_RemoveIsCalled()
        {
            // Arrange
            var id = 1;

            // Act
            controller.Delete(id);
            
            // Assert

            mockService.Verify(s => s.Remove(id), Times.Once);
            
        }

        [Fact]
        public void Delete_WhenIdIsLessThanOne_RemoveIsNotCalled()
        {
            // Arrange
            var id = 0;
            
            // Act
            controller.Delete(id);

            // Assert
            mockService.Verify(s => s.Remove(id), Times.Never);
        }

        
    }
}
