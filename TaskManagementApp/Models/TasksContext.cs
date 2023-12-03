using Microsoft.EntityFrameworkCore;

namespace TaskManagementApp.Models
{
    public class TasksContext : DbContext
    {
        public TasksContext(DbContextOptions<TasksContext> options)
        : base(options)
        {
        }

        public DbSet<Tasks> Tasks { get; set; } = null!;
    }
}
