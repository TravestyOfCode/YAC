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
            Items = p.Items.AsQueryable().ProjectToModel()
        });
    }
}
