using System.Collections.Generic;
using HotelBooking.Application.Common.Facade;
using HotelBooking.Application.Rooms.Facade;
using HotelBooking.Core;

namespace HotelBooking.Application.Rooms
{
    public class RoomsDomainService : IRoomsDomainService
    {
        private IRepository<Room> _repos;

        public RoomsDomainService(IRepository<Room> repos) {
            _repos = repos;
        }

        public IEnumerable<Room> GetAll()
        {
            return _repos.GetAll();
        }

        public void Remove(int id)
        {
            _repos.Remove(id);
        }

        public Room Get(int id)
        {
            return _repos.Get(id);
        }
    }
}