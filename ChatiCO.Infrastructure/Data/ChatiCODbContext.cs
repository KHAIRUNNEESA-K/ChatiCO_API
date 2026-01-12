using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatiCO.Domain.Entities;

namespace ChatiCO.Infrastructure.Data
{
    public class ChatiCODbContext : DbContext
    {
        public ChatiCODbContext(DbContextOptions<ChatiCODbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Archive> Archives { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<GroupMessage> GroupMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Archive>().ToTable("ArchivedChats");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChatiCODbContext).Assembly);
        }

    }
}
