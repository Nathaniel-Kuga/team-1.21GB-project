using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Team121GBCapstoneProject.DAL.Abstract;
using Team121GBCapstoneProject.Services.Concrete;
using Team121GBCapstoneProject.Services.Abstract;
using Team121GBCapstoneProject.Utilities;
using Team121GBCapstoneProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Team121GBCapstoneProject.DAL.Concrete;
using Team121GBCapstoneProject.Models.DTO;
using Team121GBCapstoneProject.Models;
using Humanizer.Localisation;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Team121GBCapstoneProject.Controllers
{
    public class SteamGamesController : Controller
    {
        private readonly ILogger<SteamGamesController> _logger;
        private readonly IsteamService _steamService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPersonListRepository _personListRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IRepository<PersonGame> _personGameRepository;
        private string _clientId;
        private string _bearerToken;
        private readonly IConfiguration _config;
        private readonly IIgdbService _igdbService;
        private readonly ISteamChecker _steamChecker;


        public SteamGamesController(ILogger<SteamGamesController> logger, IsteamService steamService, UserManager<ApplicationUser> userManager, IPersonListRepository personListRepository, IGameRepository gameRepository, IRepository<PersonGame> personGameRepository, IConfiguration configuration, IIgdbService igdbService, ISteamChecker steamChecker)
        {
            _logger = logger;
            _steamService = steamService;
            _userManager = userManager;
            _personListRepository = personListRepository;
            _gameRepository = gameRepository;
            _personGameRepository = personGameRepository;
            _config = configuration;
            _igdbService = igdbService;
            _steamChecker = steamChecker;
        }

        [Authorize]
        public IActionResult SteamGamesBlank()
        {
            SteamHelper _steamHelper = new SteamHelper(_userManager);
            //string? userId = _userManager.GetUserId(User);
            var steamAccount = _steamHelper.GetSteamId(User);
            if (steamAccount == null)
            {
                return View("Index");
            }
            var games = _steamService.GetGames(steamAccount);
            return View("Index", games);
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            _bearerToken = _config["GamingPlatform:igdbBearerToken"];
            _clientId = _config["GamingPlatform:igdbClientId"];


            // Set Credentials
            _igdbService.SetCredentials(_clientId, _bearerToken);
            SteamHelper _steamHelper = new SteamHelper(_userManager);
            //string? userId = _userManager.GetUserId(User);
            var steamAccount = _steamHelper.GetSteamId(User);
            if (steamAccount == null)
            {
                return View();
            }
            var games = _steamService.GetGames(steamAccount);

            //start of steam games
            ApplicationUser currentUser = await _userManager.GetUserAsync(User); //use as well

            //Get all games from want to play
            /*PersonList personList = _personListRepository.GetAll()
                                                              .FirstOrDefault(pl => pl.ListKind == "Want to Play" && pl.Person.AuthorizationId == currentUser.Id);*/
            foreach(var game in games.response.games)
            {
                //Check if game it played.
                PersonList personList;
                if (game.playtime_forever > 0)
                {
                    personList = _personListRepository.GetAll()
                                                              .FirstOrDefault(pl => pl.ListKind == "Currently Playing" && pl.Person.AuthorizationId == currentUser.Id);
                }
                else
                {
                    personList = _personListRepository.GetAll()
                                                              .FirstOrDefault(pl => pl.ListKind == "Want to Play" && pl.Person.AuthorizationId == currentUser.Id);
                }


                bool check = false;

                check = _steamChecker.checkGameTitle(game.name, personList.PersonGames.ToList());

                if (check == true)
                {
                    check = false;
                    continue;
                }

                Thread.Sleep(250);
                await _igdbService.SearchGameWithCachingAsync(10,"","",0,game.name); //getting games

                var gameToAdd = _gameRepository.GetGamesByTitle(game.name).FirstOrDefault();
                var personGames = personList.PersonGames.ToList();

                if (gameToAdd == null)
                {
                    continue;
                }
                //already calling to list here.
                check = _steamChecker.checkGameId(gameToAdd.Id, personList.PersonGames.ToList());

                if (check == true)
                {
                    check = false;
                    continue;
                }

         
                PersonGame newPersonGame = new PersonGame
                {
                    PersonList = personList,
                    PersonListId = personList.Id,
                    Game = gameToAdd,
                    GameId = gameToAdd.Id
                };

                _personGameRepository.AddOrUpdate(newPersonGame);
            }



            return View(games);
        }

        [Authorize]
        public IActionResult ChatGpt() => View("../Home/ChatGpt");
    }
}
