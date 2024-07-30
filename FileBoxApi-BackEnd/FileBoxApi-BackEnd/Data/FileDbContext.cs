namespace FileBoxApi.Data
{
    using Microsoft.EntityFrameworkCore;

    using FileBoxApi.Models;

    public class FileDbContext : DbContext
    {
        public FileDbContext(DbContextOptions<FileDbContext> options)
            : base(options) { }

        public DbSet<FileRecord> Files { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileRecord>()
                .HasIndex(f => new { f.Name, f.Extension })
                .IsUnique();
        }
    }
}
