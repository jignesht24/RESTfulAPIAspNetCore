using CustomerAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.Repositories
{
    public class CustomerRepository :ICustomerRepository
    {
        private EntityContext _context;

        public CustomerRepository(EntityContext context)
        {
            _context = context;
        }
        public void AddCustomer(Customer customer)
        {
            if (customer.Id == Guid.Empty)
            {
                customer.Id = Guid.NewGuid();
            }
            customer.CreatedDate = DateTime.Now;
            _context.Customers.Add(customer);
        }

        public bool CustomerExists(Guid customerId)
        {
            return _context.Customers.Any(p => p.Id == customerId);
        }

        public void DeleteCustomer(Customer customer)
        {
            _context.Customers.Remove(customer);
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _context.Customers;
        }

        public Customer GetCustomer(Guid customerId)
        {
            return _context.Customers.FirstOrDefault(a => a.Id == customerId);
        }
        public IEnumerable<Customer> GetCustomers(IEnumerable<Guid> customerIds)
        {
            return _context.Customers.Where(a => customerIds.Contains(a.Id));
        }

        public void UpdateCustomer(Customer customer)
        {
            
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
