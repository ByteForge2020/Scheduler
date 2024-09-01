using General.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace General.Infrastructure.Configuration
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(ap => ap.Id);

            builder.Property(ap => ap.Start)
                .IsRequired();
            
            builder.HasOne(ap => ap.Account)
                .WithMany(a => a.Appointments)
                .HasForeignKey(ap => ap.AccountId);

            builder.HasOne(ap => ap.Customer)
                .WithMany(c => c.Appointments)
                .HasForeignKey(ap => ap.CustomerId);

            builder.HasOne(ap => ap.Service)
                .WithMany(s => s.Appointments)
                .HasForeignKey(ap => ap.ServiceId);

            builder.HasOne(ap => ap.Specialist)
                .WithMany(sp => sp.Appointments)
                .HasForeignKey(ap => ap.SpecialistId);
        }
    }
}