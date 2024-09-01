using General.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace General.Infrastructure
{
    public interface IGeneralDbContext
    {
        public DbSet<Account> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Specialist> Specialists { get; set; }
    }
}