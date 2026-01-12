using ChatiCO.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Infrastructure.Configurations
{
    public class GroupMessageConfiguration
    {
        public void Configure(EntityTypeBuilder<GroupMessage> builder)
        {
            builder.HasKey(m => m.GroupMessageId);

            builder.Property(m => m.GroupId)
                .IsRequired();

            builder.Property(m => m.SenderId)
                .IsRequired();

            builder.Property(m => m.MessageType)
                .IsRequired()
                .HasMaxLength(10); 

            builder.Property(m => m.Content)
                .HasMaxLength(5000);

            builder.Property(m => m.FileUrl)
                .HasMaxLength(500);

            builder.Property(m => m.FileName)
                .HasMaxLength(255);

            builder.Property(m => m.FileType)
                .HasMaxLength(100);

            builder.Property(m => m.SentTime)
                .IsRequired();
        }
    }
}
