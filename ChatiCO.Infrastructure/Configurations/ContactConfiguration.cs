using ChatiCO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatiCO.Infrastructure.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contacts>
    {
        public void Configure(EntityTypeBuilder<Contacts> builder)
        {
            builder.HasKey(c => c.ContactId);

            builder.Property(c => c.ContactId)
                .ValueGeneratedOnAdd();

            builder.HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.ContactUser)
                .WithMany()
                .HasForeignKey(c => c.ContactUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.IsBlocked)
                   .HasDefaultValue(false)
                   .IsRequired();
        }
    }
}
