using HotelBooking.Application.Rooms;
using HotelBooking.Application.Rooms.Facade;
using HotelBooking.Core;
using HotelBooking.Infrastructure.Repositories;
using HotelBooking.IntegrationTests.Fixtures;
using HotelBooking.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HotelBooking.IntegrationTests {
    public class RoomsControllerTests : IClassFixture<DatabaseFixture> {

        public RoomsController controller;

        public RoomsControllerTests(DatabaseFixture dbFixture) {
            var ctx = dbFixture.Context;
            var roomRepos = new RoomRepository(ctx);
            var roomRepository = new RoomsDomainService(roomRepos);
            controller = new RoomsController(roomRepository);

        }

        [Fact]
        public void Delete_DeleteSpecificRoom_NoContent() {
            //Arrange
            List<Room> roomsBeforeDelete = controller.Get().ToList();

            //Act
            var result = controller.Delete(1);

            List<Room> roomsAfterDelete = controller.Get().ToList();

            //Assert
            Assert.True(roomsBeforeDelete.Count > roomsAfterDelete.Count);
        }

        [Fact]
        public void Get_GetRoomById_ObjectResultWithARoom() {
            //Arrange
            var id = 1;

            //Act
            var result = controller.Get(id);

            //Assert
            var oResult = Assert.IsType<ObjectResult>(result);
            var room = Assert.IsType<Room>(oResult.Value);
            Assert.True(room.Id == id);
        }

        [Fact]
        public void Get_GetAListOfRooms_IEnumerableListOfRooms() {
            //Arrange

            //Act
            var result = controller.Get();

            //Assert
            Assert.True(result is not null);
        }

        [Fact]
        public void Delete_WhenIdIsLargerThanTwo_BadRequestObjectResult() {
            // Arrange
            var id = int.MaxValue;
            var expectedErrorMessages = "Room does not exist";

            // Act
            var result = controller.Delete(id);

            // Assert
            var badrequest = Assert.IsType<BadRequestObjectResult>(result);
            var message = Assert.IsType<string>(badrequest.Value);
            Assert.Equal(message, expectedErrorMessages);
        }
    }
}
