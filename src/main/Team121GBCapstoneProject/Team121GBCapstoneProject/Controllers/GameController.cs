using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Team121GBCapstoneProject.Areas.Identity.Data;
using Team121GBCapstoneProject.Models;
using Team121GBCapstoneProject.Services.Abstract;
using Team121GBCapstoneProject.DAL.Abstract;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Team121GBCapstoneProject.DAL.Concrete;
using Team121GBCapstoneProject.Models.DTO;

namespace Team121GBCapstoneProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIgdbService _igdbService;
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<Game> _gameRepository;
        private string _clientId;
        private string _bearerToken;
        private readonly IConfiguration _config;

        public GameController(UserManager<ApplicationUser> userManager,
                                IIgdbService igdbService,
                                IRepository<Person> personRepository,
                                IRepository<Game> gameRepository,
                                IConfiguration configuration)
        {
            _userManager = userManager;
            _igdbService = igdbService;
            _personRepository = personRepository;
            _gameRepository = gameRepository;
            _config = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IgdbGame>>> Index(string query)
        {
            // TODO: Move these to UserSecrets before deployment
            //_bearerToken = "llrnvo5vfowcyr0ggecl445q5dunyl";
            //_clientId = "8ah5b0s8sx19uadsx3b5m4bfekrgla";
            _bearerToken = _config["GamingPlatform:igdbBearerToken"];
            _clientId = _config["GamingPlatform:igdbClientId"];

            // Set Credentials
            _igdbService.SetCredentials(_clientId, _bearerToken);

            var searchResult = await _igdbService.SearchGames(query);

            if (searchResult is null)
            {
                return NotFound();
            }

            return Ok(searchResult);
        }

        [HttpGet("getUserLists")]
        public async Task<ActionResult<List<PersonGameList>>> GetUserLists()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            List<PersonGameList> userLists = _personRepository.GetAll()
                                                         .Where(user => user.AuthorizationId == currentUser.Id)
                                                         .First()
                                                         .PersonGameLists
                                                         .ToList();
            List<PersonGameListDTO> userListDTO = new List<PersonGameListDTO>();
            foreach (var list in userLists)
            {
                PersonGameListDTO listDTO = new PersonGameListDTO(list.ListName.NameOfList);
                userListDTO.Add(listDTO);  
            }
            if(userLists.Count == 0)
            {
                return BadRequest("You don't have any lists!");
            }
            return Ok(userListDTO);
        }
        [HttpPost("addGame")]
        public async Task<ActionResult<IgdbGame>> AddGameToList([Bind("Title")] Game game, string listName)
        //public async Task<ActionResult<IgdbGame>> AddGameToList(string listName)
        {   //need to set it up so we have a user a that is logged in.
            var loggedInUser = _personRepository.GetAll()
                                               .Where(user => user.AuthorizationId == _userManager.GetUserId(User))
                                               .First();
            try
            {
                // * check a user has any lists
                List<PersonGameList> userListCheck = _personRepository.GetAll()
                                                                      .Where(user => user.AuthorizationId == _userManager.GetUserId(User))
                                                                      .First()
                                                                      .PersonGameLists
                                                                      .ToList();
                if (userListCheck.Count == 0)
                {
                    return NotFound(); 
                }
                // * check if this game already exists in the database
                bool check = _gameRepository.GetAll()
                                            .Any(g => g.Title == game.Title);
                if (check) //add game to a users list 
                {
                    check = loggedInUser.PersonGameLists
                                        .Where(g => g.Game != null)
                                        .Any(g => g.Game.Title == game.Title);
                    //check if user's list already contains this game.
                    if (check)
                    {
                        //return what?
                        return Content("This game is already in your list.");
                    }
                    //grab game from db and add it your specified list
                    string gameTitle = game.Title;
                    game = _gameRepository.GetAll()
                                          .Where(g => g.Title == gameTitle)
                                          .First();
                    //string listName = "Currently Playing";
                    PersonGameList list = loggedInUser.PersonGameLists
                                                      .Where(l => l.ListName.NameOfList == listName)
                                                      .First();

                    PersonGameList gameList = new PersonGameList();
                    gameList.ListName = list.ListName;
                    gameList.ListNameId = list.ListNameId;
                    gameList.Game = game;
                    gameList.GameId = game.Id;
                    gameList.Person = loggedInUser;
                    gameList.PersonId = loggedInUser.Id;
                    gameList.ListKind = list.ListKind;
                    gameList.ListKindId = list.ListKindId;

                    loggedInUser.PersonGameLists.Add(gameList);
                    _personRepository.AddOrUpdate(loggedInUser);

                }
                else // add game to db first.
                {
                    await AddGameToDb(game);
                    //now add to users list
                    //grab game from db and add it your specified list
                    string gameTitle = game.Title;
                    game = _gameRepository.GetAll()
                                          .Where(g => g.Title == gameTitle)
                                          .First();
                    //string listName = "Currently Playing";
                    PersonGameList list = loggedInUser.PersonGameLists
                                                      .Where(l => l.ListName.NameOfList == listName)
                                                      .First();

                    PersonGameList gameList = new PersonGameList();
                    gameList.ListName = list.ListName;
                    gameList.ListNameId = list.ListNameId;
                    gameList.Game = game;
                    gameList.GameId = game.Id;
                    gameList.Person = loggedInUser;
                    gameList.PersonId = loggedInUser.Id;
                    gameList.ListKind = list.ListKind;
                    gameList.ListKindId = list.ListKindId;

                    loggedInUser.PersonGameLists.Add(gameList);
                    _personRepository.AddOrUpdate(loggedInUser);
                }
                return Ok();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<IgdbGame>> AddGameToDb(Game game)
        {
            try
            {
                _gameRepository.AddOrUpdate(game);
                return Ok();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return BadRequest(e);
            }
        }
    }
}
