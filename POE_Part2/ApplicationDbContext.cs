using Microsoft.EntityFrameworkCore;

namespace POE_Part2
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
            optionsBuilder.UseLazyLoadingProxies();
        }

        // ADD THIS METHOD TO CREATE THE DATABASE
        public void EnsureDatabaseCreated()
        {
            Database.EnsureCreated();
        }
    }
}