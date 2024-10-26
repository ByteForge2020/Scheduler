using Microsoft.EntityFrameworkCore;

namespace Customer.Infrastructure
{
    public interface ICustomerDbContext
    {
        public DbSet<Core.Entities.Customer> Customers { get; set; }
    }
}