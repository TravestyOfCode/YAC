using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAC.Data;
using YAC.Models;

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
}
