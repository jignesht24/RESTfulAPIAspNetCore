using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerAPI.Entities;
using CustomerAPI.Model;
using CustomerAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CustomerAPI.Controllers
{
    [Route("api/Customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        ICustomerRepository _repository;
        public CustomerController(ICustomerRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetCustomer()
        {
            var customers = _repository.GetCustomers();
            var allCustomers = AutoMapper.Mapper.Map<IEnumerable<CustomerModel>>(customers);
            return Ok(allCustomers);
        }

        [HttpGet("{customerId}", Name = "GetCustomer")]
        public IActionResult GetCustomer(Guid customerId)
        {
            var customer = _repository.GetCustomer(customerId);
            if (customer == null)
            {
                return NotFound();
            }
            var newCustomer = AutoMapper.Mapper.Map<CustomerModel>(customer);
            return Ok(newCustomer);
        }
        [HttpPost]
        public IActionResult CreateCustomer([FromBody] CustomerCreateModel customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }
            var customerEntity = AutoMapper.Mapper.Map<Customer>(customer);

            _repository.AddCustomer(customerEntity);

            if (!_repository.Save())
            {
                return StatusCode(500, "Server Error");
            }

            var returnEntity = AutoMapper.Mapper.Map<CustomerModel>(customerEntity);

            return CreatedAtRoute("GetCustomer", new { customerId = returnEntity.Id }, returnEntity);

        }

        [HttpDelete("{ids}")]
        public IActionResult DeleteCustomers([ModelBinder(BinderType = typeof(StringToArrayModelBinder))]IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }
            foreach (var id in ids)
            {
                var customer = _repository.GetCustomer(id);
                if (customer == null)
                {
                    return NotFound();
                }
                _repository.DeleteCustomer(customer);
            }


            if (!_repository.Save())
            {
                return StatusCode(500, "Server Error");
            }
            return NoContent();
        }
        //[HttpPut("{id}")]
        //public IActionResult UpdateCustomer(Guid id, [FromBody] CustomerUpdateModel customer)
        //{
        //    var customerEntity = _repository.GetCustomer(id);
        //    if(customerEntity == null)
        //    {
        //        return NotFound();
        //    }
        //    AutoMapper.Mapper.Map(customer, customerEntity);
        //    _repository.UpdateCustomer(customerEntity);
        //    if (!_repository.Save())
        //    {
        //        return StatusCode(500, "Server Error");
        //    }
        //    return NoContent();
        //}

        // This is Upserting 
        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(Guid id, [FromBody] CustomerUpdateModel customer)
        {
            var customerEntity = _repository.GetCustomer(id);
            if (customerEntity == null)
            {
                customerEntity = AutoMapper.Mapper.Map<Customer>(customer);
                customerEntity.Id = id;
                _repository.AddCustomer(customerEntity);

                if (!_repository.Save())
                {
                    return StatusCode(500, "Server Error");
                }

                var returnEntity = AutoMapper.Mapper.Map<CustomerModel>(customerEntity);

                return CreatedAtRoute("GetCustomer", new { customerId = returnEntity.Id }, returnEntity);
            }
            AutoMapper.Mapper.Map(customer, customerEntity);
            _repository.UpdateCustomer(customerEntity);
            if (!_repository.Save())
            {
                return StatusCode(500, "Server Error");
            }
            return NoContent();
        }

        //[HttpPatch("{id}")]
        //public IActionResult UpdatePartialCustomer(Guid id, [FromBody] JsonPatchDocument<CustomerUpdateModel> customerPatchdoc)
        //{
        //    if(customerPatchdoc==null)
        //    {
        //        return BadRequest();
        //    }
        //    var customerEntity = _repository.GetCustomer(id);
        //    if (customerEntity == null)
        //    {
        //        return NotFound();
        //    }
        //    var customerToPatch = AutoMapper.Mapper.Map<CustomerUpdateModel>(customerEntity);
        //    customerPatchdoc.ApplyTo(customerToPatch);

        //    //Need to Validation

        //    AutoMapper.Mapper.Map(customerToPatch, customerEntity);

        //    _repository.UpdateCustomer(customerEntity);
        //    if (!_repository.Save())
        //    {
        //        return StatusCode(500, "Server Error");
        //    }
        //    return NoContent();

        //}

        // This is Upserting 
        [HttpPatch("{id}")]
        public IActionResult UpdatePartialCustomer(Guid id, [FromBody] JsonPatchDocument<CustomerUpdateModel> customerPatchdoc)
        {
            if (customerPatchdoc == null)
            {
                return BadRequest();
            }
            var customerEntity = _repository.GetCustomer(id);
            if (customerEntity == null)
            {
                var customerUpdate = new CustomerUpdateModel();
                customerPatchdoc.ApplyTo(customerUpdate);
                customerEntity = AutoMapper.Mapper.Map<Customer>(customerUpdate);

                customerEntity.Id = id;
                _repository.AddCustomer(customerEntity);

                if (!_repository.Save())
                {
                    return StatusCode(500, "Server Error");
                }

                var returnEntity = AutoMapper.Mapper.Map<CustomerModel>(customerEntity);

                return CreatedAtRoute("GetCustomer", new { customerId = returnEntity.Id }, returnEntity);
            }
            var customerToPatch = AutoMapper.Mapper.Map<CustomerUpdateModel>(customerEntity);
            customerPatchdoc.ApplyTo(customerToPatch);

            //Need to Validation

            AutoMapper.Mapper.Map(customerToPatch, customerEntity);

            _repository.UpdateCustomer(customerEntity);
            if (!_repository.Save())
            {
                return StatusCode(500, "Server Error");
            }
            return NoContent();

        }
    }
}