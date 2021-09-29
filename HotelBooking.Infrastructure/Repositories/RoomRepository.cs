﻿using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Application.Common.Facade;
using HotelBooking.Core;

namespace HotelBooking.Infrastructure.Repositories
{
    public class RoomRepository : IRepository<Room>
    {
        private readonly HotelBookingContext db;

        public RoomRepository(HotelBookingContext context)
        {
            db = context;
        }

        public void Add(Room entity)
        {
            db.Room.Add(entity);
            db.SaveChanges();
        }

        public void Edit(Room entity)
        {
            db.Room.Update(entity);
            db.SaveChanges();
        }

        public Room Get(int id)
        {
            // The FirstOrDefault method below returns null
            // if there is no room with the specified Id.
            return db.Room.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Room> GetAll()
        {
            return db.Room.ToList();
        }

        public void Remove(int id)
        {
            // The Single method below throws an InvalidOperationException
            // if there is not exactly one room with the specified Id.
            var room = db.Room.FirstOrDefault(r => r.Id == id);
            
            if (room is null) {
                throw new InvalidOperationException("Room does not exist");
            }

            db.Room.Remove(room);
            db.SaveChanges();
        }
    }
}
