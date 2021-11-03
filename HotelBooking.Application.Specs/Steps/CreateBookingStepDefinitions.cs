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


        private DateTime _before = new DateTime(2021, 11, 4);
        private DateTime _occupied = new DateTime(2021, 11, 5);
        private DateTime _after = new DateTime(2021, 11, 11);

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
                }
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

        #region MyRegion

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

        [Given(@"the booking starts before an occupied period")]
        public void GivenTheBookingStartsBeforeAOccupiedPeriod()
        {
            _bookingStart = _before;
        }

        [Given(@"the boooking ends before an occupied period")]
        public void GivenTheBoookingEndsBeforeAOccupiedPeriod()
        {
            _bookingEnd = _before;
        }

        [Then(@"it should be created successfully")]
        public void ThenIsShouldBeCreatedSuccessfully()
        {
            Assert.True(_result);
        }

        [Given(@"the booking starts after an occupied period")]
        public void GivenTheBookingStartsAfterAOccupiedPeriod()
        {
            _bookingStart = _after;
        }

        [Given(@"the booking ends after an occupied period")]
        public void GivenTheBoookingEndsAfterAOccupiedPeriod()
        {
            _bookingEnd = _after;
        }

        [Then(@"it should not be created")]
        public void ThenItShouldNotBeCreated()
        {
            Assert.False(_result);
        }

        [Given(@"the booking ends in an occupied period")]
        public void GivenTheBookingEndsInAnOccupiedPeriod()
        {
            _bookingStart = _occupied;
        }

        [Given(@"the booking starts in an occupied period")]
        public void GivenTheBookingStartsInAnOccupiedPeriod()
        {
            _bookingStart = _occupied;
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

        #endregion


/*        
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

        [Then(@"is should be created in the system, '(.*)'")]
        public void ThenIsShouldBeCreatedInTheSystem(bool result)
        {
            if (_thrown != null)
            {
                throw _thrown;
            }

            Assert.Equal(_result, result);
        }
        */
    }
}