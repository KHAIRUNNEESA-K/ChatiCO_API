using ChatiCO.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Infrastructure.Configurations
{
    public class GroupMemberConfiguration
    {
        public void Configure(EntityTypeBuilder<GroupMember> builder)
        {
            builder.HasKey(m => m.GroupMemberId);

            builder.Property(m => m.GroupId)
                .IsRequired();

            builder.Property(m => m.UserId)
                .IsRequired();

            builder.Property(m => m.JoinedAt)
                .IsRequired();

            builder.HasIndex(m => new { m.GroupId, m.UserId })
                .IsUnique();
        }
    }
}
