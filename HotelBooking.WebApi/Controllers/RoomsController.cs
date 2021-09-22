﻿using System;
using System.Collections.Generic;
using HotelBooking.Application.Common.Facade;
using HotelBooking.Application.Rooms.Facade;
using HotelBooking.Core;
using Microsoft.AspNetCore.Mvc;


namespace HotelBooking.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : Controller
    {
        private readonly IRoomsDomainService repository;

        public RoomsController(IRoomsDomainService repos)
        {
            repository = repos;
        }

        // GET: api/rooms
        [HttpGet]
        public IEnumerable<Room> Get()
        {
            return repository.GetAll();
        }

        // GET api/rooms/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // DELETE api/rooms/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id > 0)
            {
                repository.Remove(id);
                return NoContent();
            }
            else {
                return BadRequest();
            }
        }

    }
}
