using Microsoft.EntityFrameworkCore;
using FileTidyBase.Models;
using FileTidyDatabase.Models;

namespace FileTidyDatabase
{
    public class FileTidyDbContext : DbContext
    {
        public DbSet<SortFolderModel> SortFolders { get; set; }
        public DbSet<BasicSetting> BasicSettings { get; set; }

        public FileTidyDbContext(DbContextOptions<FileTidyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // SortFolderModel mapping
            modelBuilder.Entity<SortFolderModel>(entity =>
            {
                entity.HasKey(e => e.GUID);
                entity.Property(e => e.GUID).ValueGeneratedNever();
                entity.Property(e => e.FolderPath).IsRequired();
                entity.Property(e => e.Name);
            });

            // BasicSetting mapping
            modelBuilder.Entity<BasicSetting>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Key).IsRequired().HasMaxLength(128);
                entity.Property(e => e.Value).HasMaxLength(1024);
            });
        }
    }
}
