using General.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace General.Infrastructure.Configuration
{
    public class SpecialistConfiguration : IEntityTypeConfiguration<Specialist>
    {
        public void Configure(EntityTypeBuilder<Specialist> builder)
        {
            builder.HasKey(sp => sp.Id);

            builder.Property(sp => sp.Name)
                .IsRequired()
                .HasMaxLength(255);

            // Configure relationships
            builder.HasMany(sp => sp.Appointments)
                .WithOne(ap => ap.Specialist)
                .HasForeignKey(ap => ap.SpecialistId);
        }
    }
}