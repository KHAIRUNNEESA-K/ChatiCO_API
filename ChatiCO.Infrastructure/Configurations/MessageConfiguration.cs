using ChatiCO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Infrastructure.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");

            builder.HasKey(m => m.MessageId);

            builder.Property(m => m.MessageType)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(m => m.Content)
       .HasColumnType("VARBINARY(MAX)");  

            builder.Property(m => m.FileUrl)
                   .HasMaxLength(500);

            builder.Property(m => m.FileName)
                   .HasMaxLength(255);

            builder.Property(m => m.FileType)
                   .HasMaxLength(100);

            builder.Property(m => m.SentTime)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(m => m.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasOne<User>()
                   .WithMany(u => u.SentMessages)
                   .HasForeignKey(m => m.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>()
                   .WithMany(u => u.ReceivedMessages)
                   .HasForeignKey(m => m.ReceiverId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
