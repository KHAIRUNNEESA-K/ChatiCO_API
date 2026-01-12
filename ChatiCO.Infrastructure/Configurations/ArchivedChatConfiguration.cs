using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ArchivedChatConfiguration : IEntityTypeConfiguration<Archive>
{
    public void Configure(EntityTypeBuilder<Archive> builder)
    {
        builder.HasKey(a => a.ArchiveId);

        builder.Property(a => a.UserId).IsRequired();
        builder.Property(a => a.ContactId).IsRequired();

        builder.HasOne(a => a.User)
               .WithMany(u => u.Archives)
               .HasForeignKey(a => a.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Contact)
               .WithMany()
               .HasForeignKey(a => a.ContactId)
               .OnDelete(DeleteBehavior.Restrict);

      
        builder.Property(a => a.IsArchived)
               .HasDefaultValue(true)
               .IsRequired();

        builder.ToTable("ArchivedChats");
    }
}
