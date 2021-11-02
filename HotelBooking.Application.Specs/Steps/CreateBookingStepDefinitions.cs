using System;
using System.Collections.Generic;
using HotelBooking.Application.Bookings;
using HotelBooking.Application.Bookings.Facade;
using HotelBooking.Application.Common.Facade;
using HotelBooking.Core;
using Moq;
using TechTalk.SpecFlow;
using Xunit;

namespace HotelBooking.Application.Specs.Steps
{
    [Binding]
    public sealed class CreateBookingStepDefinitions
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef
        private IBookingDomainService _bookingDomainService;
        private Mock<IRepository<Booking>> _bookRepoMock;
        private Mock<IRepository<Room>> _roomRepoMock;
        private Mock<IDateTimeService> _dateTimeMock;
        private DateTime _today;
        private readonly ScenarioContext _scenarioContext;
        private DateTime _bookingStart;
        private DateTime _bookingEnd;
        private bool _result;
        private Exception _thrown;

        public CreateBookingStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;

            SetUp();
        }

        private void SetUp()
        {
            // Var
            _today = new DateTime(2021, 11, 1);

            _bookRepoMock = new Mock<IRepository<Booking>>();
            _roomRepoMock = new Mock<IRepository<Room>>();
            _dateTimeMock = new Mock<IDateTimeService>();


            DateTime availableRoomPeriodStart1 = new DateTime(2021, 11, 5);
            DateTime availableRoomPeriodEnd1 = new DateTime(2021, 11, 10);
            DateTime availableRoomPeriodStart2 = new DateTime(2021, 11, 15);
            DateTime availableRoomPeriodEnd2 = new DateTime(2021, 11, 20);

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

            // Setup Methods
            _dateTimeMock.Setup(s => s.Today).Returns(_today);
            _bookRepoMock.Setup(r => r.GetAll()).Returns(bookings);
            _roomRepoMock.Setup(r => r.GetAll()).Returns(rooms);

            // Build
            _bookingDomainService =
                new BookingDomainService(_bookRepoMock.Object, _roomRepoMock.Object, _dateTimeMock.Object);
        }


        [Given(@"the booking should start at '(.*)'")]
        public void GivenTheBookingShouldStartAt(string startAt)
        {
            _bookingStart = DateTime.Parse(startAt);
        }

        [Given(@"booking should end at '(.*)'")]
        public void GivenBookingShouldEndAt(string endAt)
        {
            _bookingEnd = DateTime.Parse(endAt);
        }

        [When(@"the booking is placed")]
        public void WhenTheBookingIsPlaced()
        {
            var booking = new Booking {StartDate = _bookingStart, EndDate = _bookingEnd};

            try
            {
                _result = _bookingDomainService.CreateBooking(booking);
            }
            catch (Exception e)
            {
                _thrown = e;
            }
        }

        [Then(@"is should be created in the system, '(.*)'")]
        public void ThenIsShouldBeCreatedInTheSystemFalse(bool result)
        {
            if (_thrown != null)
            {
                throw _thrown;
            }

            Assert.Equal(_result, result);
        }

        [Given(@"the booking period starts before today")]
        public void GivenTheBookingPeriodStartsBeforeToday()
        {
            
            _bookingStart = new DateTime(2021, 10, 1);
            _bookingEnd = new DateTime(2021, 11, 14);
        }

        [Then(@"booking not placed and exception is thrown")]
        public void ThenBookingNotPlacedAndExceptionIsThrown()
        {
            if (_thrown is null)
            {
                Assert.True(false);
            }
            Assert.IsType<ArgumentException>(_thrown);
        }

        [Given(@"the booking period is starts before it ends")]
        public void GivenTheBookingPeriodIsStartsBeforeItEnds()
        {
            _bookingStart = new DateTime(2021, 11, 14);
            _bookingEnd = new DateTime(2021, 11, 11);
        }
    }
}