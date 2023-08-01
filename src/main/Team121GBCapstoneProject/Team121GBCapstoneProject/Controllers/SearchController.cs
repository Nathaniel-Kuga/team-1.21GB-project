using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Team121GBCapstoneProject.Controllers;

public class SearchController : Controller
{
    private readonly ILogger<SearchController> _logger;

    public SearchController(ILogger<SearchController> logger)
    {
        _logger = logger;
    }

    [Authorize]
    public IActionResult Results()
    {
        return View("SearchResults");
    }
    
    [Authorize]
    public IActionResult ChatGpt () => View("../Home/ChatGpt");
}

