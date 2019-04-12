using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerAPI.Entities;
using CustomerAPI.Model;
using CustomerAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerAPI.Controllers
{
    [Route("api/customercollection")]
    [ApiController]
    public class CustomerCollectionController : ControllerBase
    {
        ICustomerRepository _repository;
        public CustomerCollectionController(ICustomerRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{ids}", Name = "GetCustomerCollection")]
        public IActionResult GetCustomers([ModelBinder(BinderType = typeof(StringToArrayModelBinder))]IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }
            var customerEntities = _repository.GetCustomers(ids);

            var customers = AutoMapper.Mapper.Map<IEnumerable<CustomerModel>>(customerEntities);

            return Ok(customers);
        }
        [HttpPost]
        public IActionResult AddCustomers([FromBody] IEnumerable<CustomerCreateModel> customers)
        {
            if (customers == null)
            {
                return BadRequest();
            }

            var customerEntities = AutoMapper.Mapper.Map<IEnumerable<Customer>>(customers);
            foreach (var customer in customerEntities)
            {
                _repository.AddCustomer(customer);
            }
            if (!_repository.Save())
            {
                return StatusCode(500, "Server Error");
            }
            var allCustomer = AutoMapper.Mapper.Map<IEnumerable<CustomerModel>>(customerEntities);

            return CreatedAtRoute("GetCustomerCollection", new { ids = string.Join(',', allCustomer.Select(p => p.Id)) }, allCustomer);
        }
    }
}