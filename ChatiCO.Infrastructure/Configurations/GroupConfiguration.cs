using ChatiCO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Infrastructure.Configurations
{
    public class GroupConfiguration
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(g => g.GroupId);

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(g => g.ProfileImageUrl)
                .HasMaxLength(500);

            builder.Property(g => g.CreatedByUserId)
                .IsRequired();

            builder.HasMany(g => g.Members)
                .WithOne(m => m.Group)
                .HasForeignKey(m => m.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(g => g.Messages)
                .WithOne(msg => msg.Group)
                .HasForeignKey(msg => msg.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
