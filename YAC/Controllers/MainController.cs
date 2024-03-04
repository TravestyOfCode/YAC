using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAC.Data;
using YAC.Models;
using YAC.Utilities;

namespace YAC.Controllers;

[Authorize]
public class MainController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    private readonly ILogger<MainController> _logger;

    public MainController(ApplicationDbContext dbContext, ILogger<MainController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        try
        {
            // Get all of the ToDoList, with Items, for the user.
            var lists = await _dbContext.ToDoList
                .Where(p => p.OwnerId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                .Include(p => p.Items)
                .ProjectToModel()
                .ToListAsync(cancellationToken);

            return View(lists);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return StatusCode(500);
        }
    }

    [HttpGet]
    public async Task<IActionResult> EditingTitle(int id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var entity = await _dbContext.ToDoList
            .Where(p => p.Id.Equals(id) && p.OwnerId.Equals(userId))
            .ProjectToModel()
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            return NotFound();
        }

        return PartialView(ViewNames.EditingTitle, entity);
    }

    [HttpPost]
    public async Task<IActionResult> EditingTitle(int id, string title, CancellationToken cancellationToken)
    {
        if (title == null || title.Length > 64)
        {
            return BadRequest();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var entity = await _dbContext.ToDoList
            .SingleOrDefaultAsync(p => p.Id.Equals(id) && p.OwnerId.Equals(userId), cancellationToken);

        if (entity == null)
        {
            return NotFound();
        }

        entity.Title = title;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return PartialView(ViewNames.EditableTitle, entity.AsModel());
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCompleted(int id, string isCompleted = "off", CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var entity = await _dbContext.ToDoItems
                .SingleOrDefaultAsync(p => p.Id.Equals(id) && p.ToDoList.OwnerId.Equals(userId), cancellationToken);

            if (entity == null)
            {
                return NotFound();
            }

            entity.IsCompleted = isCompleted.Equals("on", StringComparison.OrdinalIgnoreCase);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return StatusCode(500);
        }
    }

    // Gets an editable form for the specified item row.
    [HttpGet]
    public async Task<IActionResult> EditingItem(int id, CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var entity = await _dbContext.ToDoItems
                .Where(p => p.Id.Equals(id) && p.ToDoList.OwnerId.Equals(userId))
                .ProjectToModel()
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                return NotFound();
            }

            return PartialView(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return StatusCode(500);
        }
    }

    // Cancels an editable form for the specified item row, returning an Editable row.
    [HttpGet]
    public async Task<IActionResult> EditableItem(int id, CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var entity = await _dbContext.ToDoItems
                .Where(p => p.Id.Equals(id) && p.ToDoList.OwnerId.Equals(userId))
                .ProjectToModel()
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                return NotFound();
            }

            return PartialView(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditableItem(int id, string description, DateTime? dueBy, CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var entity = await _dbContext.ToDoItems
                .SingleOrDefaultAsync(p => p.Id.Equals(id) && p.ToDoList.OwnerId.Equals(userId), cancellationToken);

            if (entity == null)
            {
                return NotFound();
            }

            entity.Description = description;
            entity.DueBy = dueBy;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return PartialView(entity.AsModel());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error.");

            return StatusCode(500);
        }
    }
}
