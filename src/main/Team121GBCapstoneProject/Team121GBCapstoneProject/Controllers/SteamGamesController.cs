using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Team121GBCapstoneProject.DAL.Abstract;
using Team121GBCapstoneProject.Services.Concrete;
using Team121GBCapstoneProject.Services.Abstract;
using Team121GBCapstoneProject.Utilities;
using Team121GBCapstoneProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace Team121GBCapstoneProject.Controllers
{
    public class SteamGamesController : Controller
    {
        private readonly ILogger<SteamGamesController> _logger;
        private readonly IsteamService _steamService;
        private readonly UserManager<ApplicationUser> _userManager;
        public SteamGamesController(ILogger<SteamGamesController> logger, IsteamService steamService, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _steamService= steamService;
            _userManager= userManager;
        }
        [Authorize]
        public IActionResult Index()
        {
            SteamHelper _steamHelper = new SteamHelper(_userManager);
            //string? userId = _userManager.GetUserId(User);
            var steamAccount = _steamHelper.GetSteamId(User);
            if (steamAccount == null)
            {
                return View();
            }
            var games = _steamService.GetGames(steamAccount);
            return View(games);
        }
    }
}
