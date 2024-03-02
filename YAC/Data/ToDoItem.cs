using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YAC.Data;

public class ToDoItem
{
    public int Id { get; set; }

    public int ToDoListId { get; set; }

    public ToDoList ToDoList { get; set; }

    public bool IsCompleted { get; set; }

    public string Description { get; set; }

    public DateTimeOffset? DueBy { get; set; }
}

public class ToDoItemConfiguration : IEntityTypeConfiguration<ToDoItem>
{
    public void Configure(EntityTypeBuilder<ToDoItem> builder)
    {
        builder.ToTable(nameof(ToDoItem));

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Description)
            .HasMaxLength(256)
            .IsRequired(true);

        builder.Property(p => p.DueBy)
            .IsRequired(false);

        builder.HasOne(p => p.ToDoList)
            .WithMany(p => p.Items)
            .HasPrincipalKey(p => p.Id)
            .HasForeignKey(p => p.ToDoListId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
