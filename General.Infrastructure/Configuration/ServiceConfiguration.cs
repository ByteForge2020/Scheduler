using General.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace General.Infrastructure.Configuration
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(s => s.Price)
                .HasColumnType("decimal(10, 2)")
                .IsRequired();
            
            builder.HasMany(s => s.Appointments)
                .WithOne(ap => ap.Service)
                .HasForeignKey(ap => ap.ServiceId);
        }
    }
}