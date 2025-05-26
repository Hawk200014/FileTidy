using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FileTidyDatabase
{
    public class DesignTimeFileTidyDbContextFactory : IDesignTimeDbContextFactory<FileTidyDbContext>
    {
        public FileTidyDbContext CreateDbContext(string[] args)
        {
            var dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "data");
            Directory.CreateDirectory(dataDirectory);

            var dbPath = Path.Combine(dataDirectory, "data.db");
            var connectionString = $"Data Source={dbPath}";

            var optionsBuilder = new DbContextOptionsBuilder<FileTidyDbContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new FileTidyDbContext(optionsBuilder.Options);
        }
    }
}
