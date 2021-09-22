using System.Collections.Generic;
using HotelBooking.Core;

namespace HotelBooking.Application.Customers.Facade
{
    public interface ICustomerDomainService
    {
        IEnumerable<Customer> GetAll();
        Customer Get(int id);
        void Add(Customer customer);
        void Update(Customer customer);
        void Remove(int id);
    }
}