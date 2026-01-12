using ChatiCO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ChatiCO.Infrastructure.Data
{
    public class ChatiCODbContextFactory : IDesignTimeDbContextFactory<ChatiCODbContext>
    {
        public ChatiCODbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ChatiCODbContext>();

            optionsBuilder.UseSqlServer("Server=LENOVO\\MSSQLSERVER02;Database=ChatiCO_DB;Trusted_Connection=True;TrustServerCertificate=true");

            return new ChatiCODbContext(optionsBuilder.Options);
        }
    }
}
