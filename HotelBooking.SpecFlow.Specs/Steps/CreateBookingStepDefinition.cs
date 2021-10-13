using System;
using System.Text.RegularExpressions;
using HotelBooking.Application.Bookings;
using HotelBooking.Application.Bookings.Facade;
using HotelBooking.Core;
using TechTalk.SpecFlow;

namespace HotelBooking.SpecFlow.Specs.Steps
{
    [Binding]
    public sealed class CreateBookingStepDefinition
    {
        private readonly Booking _booking;
        public CreateBookingStepDefinition(ScenarioContext scenarioContext)
        {
        }

        [Given("The startDate is (.*)")]
        public void GivenTheStartDate(string startDate)
        {
            DateTime date = new DateTime();
            //Given format MM-dd-yyyy
            date.AddDays(Convert.ToInt32(startDate.Split("-")[1]));
            date.AddMonths(Convert.ToInt32(startDate.Split("-")[0]));
            date.AddYears(Convert.ToInt32(startDate.Split("-")[2]));

            _booking.StartDate = date;
        }
        
        [Given("The endDate is (.*)")]
        public void GivenEnddate(string endDate)
        {
            DateTime date = new DateTime();
            //Given format MM-dd-yyyy
            date.AddDays(Convert.ToInt32(endDate.Split("-")[1]));
            date.AddMonths(Convert.ToInt32(endDate.Split("-")[0]));
            date.AddYears(Convert.ToInt32(endDate.Split("-")[2]));

            _booking.EndDate = date;
        }
        
    }
}