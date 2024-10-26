using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customer.Infrastructure.Configuration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Core.Entities.Customer>
    {
        public void Configure(EntityTypeBuilder<Core.Entities.Customer> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Name)
                .IsRequired() 
                .HasMaxLength(300); 

            builder.Property(e => e.Surname)
                .IsRequired() 
                .HasMaxLength(300); 

            builder.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(30);
        }
    }
}