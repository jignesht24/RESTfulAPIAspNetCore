using CustomerAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.Repositories
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetCustomers();
        IEnumerable<Customer> GetCustomers(IEnumerable<Guid> ids);
        Customer GetCustomer(Guid customerId);
        void AddCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        bool CustomerExists(Guid CustomerId);
        bool Save();
    }
}
