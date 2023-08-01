using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Team121GBCapstoneProject.Areas.Identity.Data;
using Team121GBCapstoneProject.Models;
using Team121GBCapstoneProject.Models.DTO;
using Team121GBCapstoneProject.Services.Abstract;
using Team121GBCapstoneProject.DAL.Abstract;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Team121GBCapstoneProject.DAL.Concrete;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace Team121GBCapstoneProject.Controllers
{
    [Route("api/[controller]")]
    [ExcludeFromCodeCoverage]
    [ApiController]
    public class GameController : ControllerBase
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

        public GameController(UserManager<ApplicationUser> userManager,
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IgdbGame>>> Index(string query,
                                                                     string platform = "",
                                                                     string genre = "",
                                                                     int esrbRating = 0)
        {
            try
            {
                _bearerToken = _config["GamingPlatform:igdbBearerToken"];
                _clientId = _config["GamingPlatform:igdbClientId"];

                // Set Credentials
                _igdbService.SetCredentials(_clientId, _bearerToken);
                if (String.IsNullOrEmpty(query))
                {
                    var searchResult = await _igdbService.SearchWithFiltersOnly(platform,
                                                                                 genre,
                                                                                 esrbRating);
                    if (searchResult is null)
                    {
                        return NotFound();
                    }
                    return Ok(searchResult);
                }
                else
                {
                    var searchResult = await _igdbService.SearchGameWithCachingAsync(10,
                                                                                     platform,
                                                                                     genre,
                                                                                     esrbRating,
                                                                                     query);
                    if (searchResult is null)
                    {
                        return NotFound();
                    }

                    return Ok(searchResult);
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return BadRequest("Something went wrong. Please try again.");
            }
        }

        [HttpGet("DisplaySpeedSearch")]
        public async Task<ActionResult<IEnumerable<IgdbGame>>> Index(string query)
        {
            return Ok();
        }

        [HttpPost("SpeedSearch")]
        public async Task<ActionResult<IEnumerable<IgdbGame>>> SpeedSearchForGames(string query)
        {
            _bearerToken = _config["GamingPlatform:igdbBearerToken"];
            _clientId = _config["GamingPlatform:igdbClientId"];


            // Set Credentials
            _igdbService.SetCredentials(_clientId, _bearerToken);

            var resultOfSearch = await _speedSearch.SpeedSearchingAsync(query);

            if (resultOfSearch is null)
            {
                return NotFound();
            }

            return Ok(resultOfSearch);

        }

        [HttpGet("getUserLists")]
        public async Task<ActionResult<List<PersonList>>> GetUserLists()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);
            List<PersonList> personLists = _personListRepository.GetAll()
                                                                .Where(l => l.Person.AuthorizationId == currentUser.Id)
                                                                .ToList();
            List<PersonListDTO> personListDTOs = new List<PersonListDTO>();
            personLists.ForEach(list => personListDTOs.Add(new PersonListDTO(list.ListKind)));
            return Ok(personListDTOs);

        }

        [HttpPost("addGame")]
        public async Task<ActionResult<IgdbGame>> AddGameToList([Bind("GameTitle,ImageSrc,ListKind,IgdbID")] GameDto gameDto)
        {
            try
            {
                ApplicationUser currentUser = await _userManager.GetUserAsync(User); //use as well
                // *check if the already have the game in their list
                // gets all games
                bool check = _personListRepository.GetAll()
                                              .FirstOrDefault(pl => pl.ListKind == gameDto.ListKind && pl.Person.AuthorizationId == currentUser.Id)
                                              .PersonGames.Any(pg => pg.Game.Title == gameDto.GameTitle && pg.Game.IgdbgameId == gameDto.IgdbID);
                if (check)
                {
                    return BadRequest($"You already have {gameDto.GameTitle} stored in {gameDto.ListKind}.");
                }
                // * if we have gotten to this point, we can now add the game
                //Start here
                PersonList personList = _personListRepository.GetAll()
                                                              .FirstOrDefault(pl => pl.ListKind == gameDto.ListKind/*sub 3*/ && pl.Person.AuthorizationId == currentUser.Id);

                Game game = _gameRepository.GetAll().FirstOrDefault(g => g.Title == gameDto.GameTitle && g.IgdbgameId == gameDto.IgdbID);
                PersonGame newPersonGame = new PersonGame
                {
                    PersonList = personList,
                    PersonListId = personList.Id,
                    Game = game,
                    GameId = game.Id
                };

                _personGameRepository.AddOrUpdate(newPersonGame);
                //end here 
                var response = new { message = "Success!" };
                return Ok($"Succeeded in adding {gameDto.GameTitle} to {gameDto.ListKind}.");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return BadRequest("Somethine went wrong. Please try again.");
            }
        }
    }
}
