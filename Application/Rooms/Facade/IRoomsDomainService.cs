using System.Collections.Generic;
using HotelBooking.Core;

namespace HotelBooking.Application.Rooms.Facade
{
    public interface IRoomsDomainService
    {
        IEnumerable<Room> GetAll();
        void Remove(int id);
        Room Get(int id);
    }
}