using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using Team121GBCapstoneProject.Areas.Identity.Data;
using Team121GBCapstoneProject.DAL.Abstract;
using Team121GBCapstoneProject.DAL.Concrete;
using Team121GBCapstoneProject.Models;
using Team121GBCapstoneProject.Services.Abstract;
using Team121GBCapstoneProject.Services.Concrete;

namespace Team121GBCapstoneProject.Controllers
{
    public class SpeedSearchController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIgdbService _igdbService;
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<Game> _gameRepository;
        private readonly IRepository<PersonGame> _personGameRepository;
        private readonly IRepository<ListKind> _listKindRepository;
        private readonly IPersonListRepository _personListRepository;
        private readonly ISpeedSearch _speedSearch;

        private string _clientId;
        private string _bearerToken;
        private readonly IConfiguration _config;

        public SpeedSearchController(UserManager<ApplicationUser> userManager,
                                IIgdbService igdbService,
                                IRepository<Person> personRepository,
                                IRepository<Game> gameRepository,
                                IRepository<PersonGame> personGameRepository,
                                IRepository<ListKind> listKindRepository,
                                IPersonListRepository personListRepository,
                                IConfiguration configuration,
                                ISpeedSearch speedSearch)
        {
            _userManager = userManager;
            _igdbService = igdbService;
            _personRepository = personRepository;
            _gameRepository = gameRepository;
            _personGameRepository = personGameRepository;
            _listKindRepository = listKindRepository;
            _personListRepository = personListRepository;
            _config = configuration;
            _speedSearch = speedSearch;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<IgdbGame>>> SpeedSearchOutput(string GameEntry)
        {
            _bearerToken = _config["GamingPlatform:igdbBearerToken"];
            _clientId = _config["GamingPlatform:igdbClientId"];


            // Set Credentials
            _igdbService.SetCredentials(_clientId, _bearerToken);

            var resultOfSearch = await _speedSearch.SpeedSearchingAsync(GameEntry);

            if (resultOfSearch is null)
            {
                return NotFound();
            }

            return View("SpeedSearchOutput", resultOfSearch);

        }

        /*[Authorize]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<IgdbGame>>> SpeedSearchResults(string GameEntry)
        {
            _bearerToken = _config["GamingPlatform:igdbBearerToken"];
            _clientId = _config["GamingPlatform:igdbClientId"];


            // Set Credentials
            _igdbService.SetCredentials(_clientId, _bearerToken);

            var resultOfSearch = await _speedSearch.SpeedSearchingAsync(GameEntry);

            if (resultOfSearch is null)
            {
                return NotFound();
            }

            return View("~/Views/Search/SearchResults.cshtml", resultOfSearch);

        }*/
    }
}
