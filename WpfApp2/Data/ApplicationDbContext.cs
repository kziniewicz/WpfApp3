using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WpfApp2.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace WpfApp2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public ApplicationDbContext() : this(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetConnectionString("Database"))
            .Options)
        {
        }

        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Priority> Priority { get; set; }
        public DbSet<Subtasks> SubTasks { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tasks>()
                .HasKey(t => t.TaskId);
            
            modelBuilder.Entity<AuditLog>()
                .HasKey(t => t.Id);
        }
    }
}