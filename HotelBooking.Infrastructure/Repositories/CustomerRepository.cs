using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Application.Common.Facade;
using HotelBooking.Core;

namespace HotelBooking.Infrastructure.Repositories
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly HotelBookingContext db;

        public CustomerRepository(HotelBookingContext context)
        {
            db = context;
        }

        public void Add(Customer entity)
        {
            db.Customer.Add(entity);
        }

        public void Edit(Customer entity)
        {
            db.Customer.Update(entity);
        }

        public Customer Get(int id)
        {
            return db.Customer.Where(c => c.Id == id).FirstOrDefault();
        }

        public IEnumerable<Customer> GetAll()
        {
            return db.Customer.ToList();
        }

        public void Remove(int id)
        {
            var customer = db.Customer.Where(c => c.Id == id).FirstOrDefault();

            if (customer != null)
            {
                db.Customer.Remove(customer);
            }
        }
    }
}
