using System.Collections.Generic;
using HotelBooking.Application.Common.Facade;
using HotelBooking.Application.Customers.Facade;
using HotelBooking.Core;

namespace HotelBooking.Application.Customers
{
    public class CustomerDomainService : ICustomerDomainService
    {
        private IRepository<Customer> _customerRepo;
        public CustomerDomainService(IRepository<Customer> customerRepo)
        {
            _customerRepo = customerRepo;
        }
        public IEnumerable<Customer> GetAll()
        {
            return _customerRepo.GetAll();
        }
    }
}