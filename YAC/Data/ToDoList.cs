using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace YAC.Data;

public class ToDoList
{
    public int Id { get; set; }

    public string OwnerId { get; set; }

    public IdentityUser Owner { get; set; }

    public string Title { get; set; }

    public IEnumerable<ToDoItem> Items { get; set; }
}

public class ToDoListConfiguration : IEntityTypeConfiguration<ToDoList>
{
    public void Configure(EntityTypeBuilder<ToDoList> builder)
    {
        builder.ToTable(nameof(ToDoList));

        builder.HasKey(p => p.Id);

        builder.Property(p => p.OwnerId)
            .IsRequired(true);

        builder.Property(p => p.Title)
            .HasMaxLength(64)
            .IsRequired(true);

        builder.HasOne(p => p.Owner)
            .WithMany()
            .HasPrincipalKey(p => p.Id)
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Items)
            .WithOne(p => p.ToDoList)
            .HasPrincipalKey(p => p.Id)
            .HasForeignKey(p => p.ToDoListId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
