using General.Core.Entities;

namespace General.Infrastructure.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    namespace General.Core.Entities
    {
        public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
        {
            public void Configure(EntityTypeBuilder<Customer> builder)
            {
                builder.HasKey(c => c.Id);

                builder.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(255);
                
                builder.HasMany(c => c.Appointments)
                    .WithOne(ap => ap.Customer)
                    .HasForeignKey(ap => ap.CustomerId);
                
            }
        }
    }

}