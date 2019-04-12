using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.Entities
{
    public class EntityContext : DbContext
    {
        public EntityContext(DbContextOptions<EntityContext> options) :base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
    }
}
