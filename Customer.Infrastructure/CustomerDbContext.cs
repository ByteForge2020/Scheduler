using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;

namespace Customer.Infrastructure
{
    public class CustomerDbContext : DbContext, ICustomerDbContext
    {
        public CustomerDbContext() => Database.EnsureCreated();

        public DbSet<Core.Entities.Customer> Customers { get; set; }

        
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
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