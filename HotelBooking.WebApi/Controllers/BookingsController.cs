using System;
using System.Collections.Generic;
using HotelBooking.Application.Bookings.Facade;
using HotelBooking.Core;
using Microsoft.AspNetCore.Mvc;


namespace HotelBooking.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingsController : Controller
    {
        private IBookingDomainService bookingDomainService;

        public BookingsController(IBookingDomainService domainService)
        {
            bookingDomainService = domainService;
        }

        // GET: api/bookings
        [HttpGet(Name = "GetBookings")]
        public IEnumerable<Booking> Get()
        {
            return bookingDomainService.GetAll();
        }

        // GET api/bookings/5
        [HttpGet("{id}", Name = "GetBooking")]
        public IActionResult Get(int id)
        {
            try
            {
                var item = bookingDomainService.Get(id);
                if (item == null)
                {
                    return NotFound();
                }

                return new ObjectResult(item);
            }
            catch (InvalidOperationException e)
            {
                return NotFound();
            }
        }

        // POST api/bookings
        [HttpPost]
        public IActionResult Post([FromBody] Booking booking)

        {
            if (booking == null)
            {
                return BadRequest();
            }

            bool created = bookingDomainService.CreateBooking(booking);

            if (created)
            {
                return CreatedAtRoute("GetBookings", null);
            }

            return Conflict("The booking could not be created. All rooms are occupied. Please try another period.");
        }

        // PUT api/bookings/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Booking booking)
        {
            try
            {
                if (booking == null || booking.Id != id)
                {
                    return BadRequest();
                }

                var modifiedBooking = bookingDomainService.Get(id);


                if (modifiedBooking == null)
                {
                    return NotFound();
                }

                // This implementation will only modify the booking's state and customer.
                // It is not safe to directly modify StartDate, EndDate and Room, because
                // it could conflict with other active bookings.
                modifiedBooking.IsActive = booking.IsActive;
                modifiedBooking.CustomerId = booking.CustomerId;

                bookingDomainService.Edit(modifiedBooking);
                return NoContent();
            }
            catch (InvalidOperationException e)
            {
                return NotFound();
            }
        }

        // DELETE api/bookings/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (bookingDomainService.Get(id) == null)
                {
                    return NotFound();
                }


                bookingDomainService.Remove(id);

                return NoContent();
            }
            catch (InvalidOperationException e)
            {
                return NotFound();
            }
        }
    }
}