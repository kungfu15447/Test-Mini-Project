using HotelBooking.Application.Rooms.Facade;
using HotelBooking.Core;
using HotelBooking.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HotelBooking.IntegrationTests {
    public class RoomsControllerTests{

        public RoomsController _controller;

        public RoomsControllerTests(RoomsController controller) {
            _controller = controller;

        }

        [Fact]
        public void Delete_DeleteSpecificRoom_NoContent() {
            //Arrange
            List<Room> roomsBeforeDelete = _controller.Get().ToList();

            //Act
            _controller.Delete(1);

            List<Room> roomsAfterDelete = _controller.Get().ToList();

            //Assert
            Assert.True(roomsBeforeDelete.Count > roomsAfterDelete.Count);

        }

    }
}
