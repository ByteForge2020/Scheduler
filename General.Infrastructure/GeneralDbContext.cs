using Common;
using General.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;

namespace General.Infrastructure
{
    public class GeneralDbContext : DbContext, IGeneralDbContext
    {
        public GeneralDbContext() => Database.EnsureCreated();
        public DbSet<Account> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Specialist> Specialists { get; set; }
        
        public GeneralDbContext(DbContextOptions<GeneralDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Remove pluralizing table name convention.
            foreach (IMutableEntityType entity in mb.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.DisplayName());
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                var now = DateTime.UtcNow;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}