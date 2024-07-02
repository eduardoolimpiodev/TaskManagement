using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Data
{
    public class TaskManagementContext : DbContext
    {
        public TaskManagementContext(DbContextOptions<TaskManagementContext> options)
            : base(options)
        {
        }

        public DbSet<TaskList> TaskLists { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskList>()
                .HasIndex(t => t.Name)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
