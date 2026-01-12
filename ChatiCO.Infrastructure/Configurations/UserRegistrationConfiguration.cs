using ChatiCO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ChatiCO.Infrastructure.Configurations
{
    public class UserRegistrationConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserId);
            builder.Property(u => u.UserId)
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(u => u.ProfilePicturePath)
                .HasColumnType("nvarchar(max)");

            builder.Property(u => u.Bio)
                .HasMaxLength(250);

            builder.HasIndex(u => u.PhoneNumber)
                .IsUnique();
        }
    }
}
