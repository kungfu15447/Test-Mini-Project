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

        public void Add(Customer customer)
        {
            throw new System.NotImplementedException();
        }

        public Customer Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Customer> GetAll()
        {
            return _customerRepo.GetAll();
        }

        public void Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Customer customer)
        {
            throw new System.NotImplementedException();
        }
    }
}