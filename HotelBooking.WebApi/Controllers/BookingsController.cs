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
        private IBookingManager bookingManager;

        public BookingsController(IBookingManager manager)
        {
            bookingManager = manager;
        }

        // GET: api/bookings
        [HttpGet(Name = "GetBookings")]
        public IEnumerable<Booking> Get()
        {
            return bookingManager.GetAll();
        }

        // GET api/bookings/5
        [HttpGet("{id}", Name = "GetBooking")]
        public IActionResult Get(int id)
        {
            var item = bookingManager.Get(id);
            if (item == null)
            {
                return NotFound();
            }

            return new ObjectResult(item);
        }

        // POST api/bookings
        [HttpPost]
        public IActionResult Post([FromBody] Booking booking)
        {
            if (booking == null)
            {
                return BadRequest();
            }

            bool created = bookingManager.CreateBooking(booking);

            if (created)
            {
                return CreatedAtRoute("GetBookings", null);
            }
            else
            {
                return Conflict("The booking could not be created. All rooms are occupied. Please try another period.");
            }
        }

        // PUT api/bookings/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Booking booking)
        {
            if (booking == null || booking.Id != id)
            {
                return BadRequest();
            }

            var modifiedBooking = bookingManager.Get(id);

            if (modifiedBooking == null)
            {
                return NotFound();
            }

            // This implementation will only modify the booking's state and customer.
            // It is not safe to directly modify StartDate, EndDate and Room, because
            // it could conflict with other active bookings.
            modifiedBooking.IsActive = booking.IsActive;
            modifiedBooking.CustomerId = booking.CustomerId;

            bookingManager.Edit(modifiedBooking);
            return NoContent();
        }

        // DELETE api/bookings/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (bookingManager.Get(id) == null)
            {
                return NotFound();
            }

            bookingManager.Remove(id);
            return NoContent();
        }
    }
}