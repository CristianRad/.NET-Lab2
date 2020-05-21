using Microsoft.EntityFrameworkCore;

namespace Lab2.Models
{
    public class TaskContext : DbContext
    {
        public TaskContext(DbContextOptions<TaskContext> options)
            : base(options)
        {
        }

        public DbSet<Task> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
