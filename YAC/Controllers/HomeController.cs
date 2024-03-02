using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using YAC.Models;
using YAC.Utilities;

namespace YAC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // If the user is logged in, redirect to Main
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction(controllerName: ViewNames.Main, actionName: ViewNames.Index);
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var model = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };

        _logger.LogError("Error: {model}", model);

        return View(model);
    }
}
