using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<MathTest> MathTests { get; set; }

        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<TestResultTask> TestResultTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TestResultTask>()
                .HasOne(t => t.TestResult)
                .WithMany(r => r.Tasks)
                .HasForeignKey(t => t.TestResultId);
        }
    }
}