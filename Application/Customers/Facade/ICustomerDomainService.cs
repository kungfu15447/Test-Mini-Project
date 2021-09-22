using System.Collections.Generic;
using HotelBooking.Core;

namespace HotelBooking.Application.Customers.Facade
{
    public interface ICustomerDomainService
    {
        IEnumerable<Customer> GetAll();
    }
}