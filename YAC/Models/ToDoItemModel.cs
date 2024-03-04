using System.Linq;

namespace YAC.Models;

public class ToDoItemModel
{
    public int Id { get; set; }

    public int ToDoListId { get; set; }

    public bool IsCompleted { get; set; }

    public string Description { get; set; }

    public DateTimeOffset? DueBy { get; set; }
}

public static class ToDoItemModelExtensions
{
    public static IQueryable<ToDoItemModel> ProjectToModel(this IQueryable<Data.ToDoItem> query)
    {
        return query?.Select(p => new ToDoItemModel()
        {
            Id = p.Id,
            ToDoListId = p.ToDoListId,
            IsCompleted = p.IsCompleted,
            Description = p.Description,
            DueBy = p.DueBy
        });
    }

    public static ToDoItemModel AsModel(this Data.ToDoItem entity)
    {
        if (entity == null)
        {
            return null;
        }

        return new ToDoItemModel()
        {
            Id = entity.Id,
            Description = entity.Description,
            DueBy = entity.DueBy,
            IsCompleted = entity.IsCompleted,
            ToDoListId = entity.ToDoListId
        };
    }
}