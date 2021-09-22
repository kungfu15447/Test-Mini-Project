using System;
using System.Collections.Generic;
using HotelBooking.Core;

namespace HotelBooking.Application.Bookings.Facade
{
    public interface IBookingManager
    {
        bool CreateBooking(Booking booking);
        int FindAvailableRoom(DateTime startDate, DateTime endDate);
        List<DateTime> GetFullyOccupiedDates(DateTime startDate, DateTime endDate);
        IEnumerable<Booking> GetAll();
        Booking Get(int id);
        void Remove(int id);
        void Edit(Booking modifiedBooking);
    }
}
