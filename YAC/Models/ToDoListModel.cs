using System.Collections.Generic;
using System.Linq;

namespace YAC.Models;

public class ToDoListModel
{
    public int Id { get; set; }

    public string OwnerId { get; set; }

    public string Title { get; set; }

    public IEnumerable<ToDoItemModel> Items { get; set; }
}

public static class ToDoListModelExtensions
{
    public static IQueryable<ToDoListModel> ProjectToModel(this IQueryable<Data.ToDoList> query)
    {
        return query?.Select(p => new ToDoListModel()
        {
            Id = p.Id,
            OwnerId = p.OwnerId,
            Title = p.Title,
            Items = p.Items.Select(i => new ToDoItemModel()
            {
                Id = i.Id,
                Description = i.Description,
                DueBy = i.DueBy,
                IsCompleted = i.IsCompleted,
                ToDoListId = i.ToDoListId
            })
        });
    }

    public static ToDoListModel AsModel(this Data.ToDoList entity)
    {
        if (entity == null)
        {
            return null;
        }

        return new ToDoListModel()
        {
            Id = entity.Id,
            Title = entity.Title,
            OwnerId = entity.OwnerId,
            Items = entity.Items?.Select(i => new ToDoItemModel()
            {
                Id = i.Id,
                Description = i.Description,
                DueBy = i.DueBy,
                IsCompleted = i.IsCompleted,
                ToDoListId = i.ToDoListId
            })
        };
    }
}
