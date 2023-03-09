using Microsoft.AspNetCore.Mvc;
using Team121GBCapstoneProject.DAL.Abstract;
using Team121GBCapstoneProject.Models;

namespace Team121GBCapstoneProject.Controllers
{
    public class Top100GamesController : Controller
    {
        private readonly ILogger<Top100GamesController> _logger;
        private IGameRepository _gameRepository;

        public Top100GamesController(ILogger<Top100GamesController> logger, IGameRepository gameRepo)
        {
            _logger = logger;
            _gameRepository = gameRepo;
        }

        public IActionResult Index()
        {
            GameInfo gameList = new GameInfo();
            gameList.games = _gameRepository.GetTrendingGames(100);
            gameList.backgroundColor = gameList.colorSelection(gameList.games);

            return View("Index", gameList);
        }
    }
}
