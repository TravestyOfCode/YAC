using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace YAC.Data;
public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<ToDoItem> ToDoItems { get; set; }

    public DbSet<ToDoList> ToDoList { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
    }
}
