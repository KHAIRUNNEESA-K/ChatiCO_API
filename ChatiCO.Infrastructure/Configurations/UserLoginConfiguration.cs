using ChatiCO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatiCO.Infrastructure.Persistence.Configurations
{
    public class UserLoginConfiguration : IEntityTypeConfiguration<Login>
    {
        public void Configure(EntityTypeBuilder<Login> builder)
        {
            builder.HasKey(x => x.LoginId);

            builder.Property(x => x.LoginId)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.IsSuccessful)
                   .IsRequired();

            builder.Property(x => x.LoginTime)
                   .HasColumnType("datetime");

           
            builder.HasOne(x => x.User)
                   .WithMany(u => u.Logins)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            
            builder.ToTable("Login");
        }
    }
}
