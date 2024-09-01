using General.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace General.Infrastructure.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(255);
            
            builder.HasMany(a => a.Customers)
                .WithOne(c => c.Account)
                .HasForeignKey(c => c.AccountId);

            builder.HasMany(a => a.Appointments)
                .WithOne(ap => ap.Account)
                .HasForeignKey(ap => ap.AccountId);

            builder.HasMany(a => a.Services)
                .WithOne(s => s.Account)
                .HasForeignKey(s => s.AccountId);

            builder.HasMany(a => a.Specialists)
                .WithOne(sp => sp.Account)
                .HasForeignKey(sp => sp.AccountId);
        }
    }
}