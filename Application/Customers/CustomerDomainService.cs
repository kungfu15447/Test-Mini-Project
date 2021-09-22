using System.Collections.Generic;
using HotelBooking.Application.Customers.Facade;
using HotelBooking.Core;

namespace HotelBooking.Application.Customers
{
    public class CustomerDomainService : ICustomerDomainService
    {
        public IEnumerable<Customer> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}