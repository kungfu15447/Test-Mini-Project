using System;
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
            _customerRepo.Add(customer);
        }

        public Customer Get(int id)
        {
            var customer = _customerRepo.Get(id);

            if (customer == null)
            {
                throw new ArgumentException("Could not find customer in database");
            }
            return customer;
        }

        public IEnumerable<Customer> GetAll()
        {
            return _customerRepo.GetAll();
        }

        public void Remove(int id)
        {
            _customerRepo.Remove(id);
        }

        public void Update(Customer customer)
        {
            _customerRepo.Edit(customer);
        }
    }
}