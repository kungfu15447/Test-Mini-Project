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
            controller.Delete(1);

            List<Room> roomsAfterDelete = controller.Get().ToList();

            //Assert
            Assert.True(roomsBeforeDelete.Count > roomsAfterDelete.Count);

        }

    }
}
