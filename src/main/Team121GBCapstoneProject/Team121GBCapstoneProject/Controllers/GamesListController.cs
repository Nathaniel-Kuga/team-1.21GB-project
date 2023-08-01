using Microsoft.AspNetCore.Mvc;
using Team121GBCapstoneProject.Models;
using Team121GBCapstoneProject.DAL.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Team121GBCapstoneProject.Areas.Identity.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;


namespace Team121GBCapstoneProjects.Controllers;

public class GamesListsController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<GamesListsController> _logger;
    private IPersonRepository _personRepository;
    private IPersonListRepository _personListRepository;
    private IGameRecommender _gameRecommender;
    IRepository<PersonGame> _personGameRepository;
    public GamesListsController(UserManager<ApplicationUser> userManager,
                                 ILogger<GamesListsController> logger,
                                 IPersonRepository personRepository,
                                 IPersonListRepository personListRepository,
                                 IRepository<PersonGame> personGameRepository,
                                 IGameRecommender gameRecommender)
    {
        _userManager = userManager;
        _logger = logger;
        _personRepository = personRepository;
        _personListRepository = personListRepository;
        _personGameRepository = personGameRepository;
        _gameRecommender = gameRecommender;
    }
    
    [Authorize]
    public IActionResult ChatGpt () => View("../Home/ChatGpt");
    
    [Authorize]
    [HttpGet]
    public IActionResult Index()
    {
        string authorizationId = _userManager.GetUserId(User);
        
        // gets all of the users lists
        IQueryable<PersonList> userLists = _personListRepository.GetAll()
                                                                .Join(_personRepository.GetAll(), pl => pl.PersonId, p => p.Id, (pl, p) => new { pl, p })
                                                                .Where(x => x.p.AuthorizationId == authorizationId)
                                                                .Select(x => x.pl);
        // gets all of the users games from every lists
        List<PersonGame> personGames = _personGameRepository.GetAll()
                                                            .Where(pg => pg.PersonList.Person.AuthorizationId == authorizationId)
                                                            .ToList();

        // game recommendation
        List<Game> curatedList = _gameRecommender.recommendGames(personGames,10);

        // creates dictionary where key is name of list and value is a list of games (should have 3 object that could contain more than one value)
        Dictionary<string, List<PersonGame>> personGamesByListKind = personGames.GroupBy(pg => pg.PersonList.ListKind)
                                                                                .ToDictionary(g => g.Key, g => g.ToList());

        List<PersonListVM> personListVMList = new List<PersonListVM>();

        // will loop through the dictionary and add to the list of view models an PersonListVM object (there should be 3 items in the list by default)
        // this will essentially be very similar to the dictionary but it will wrap items up in a view model object
        foreach (var kvp in personGamesByListKind)
        {
            PersonListVM temp = new PersonListVM(kvp.Key, kvp.Value);
            personListVMList.Add(temp);
        }

        // if any lists are empty there will still be PersonListVM to add to the main list to send to the view
        List<PersonList> emptyListCheck = userLists.Where(l => l.PersonGames.Count == 0)
                                                    .ToList();
        foreach (var emptyList in emptyListCheck)
        {
            PersonListVM temp = new PersonListVM(emptyList.ListKind, emptyList.PersonGames.ToList()); // ! converted PersonGames to a list because that is what the constructor expects.   
            personListVMList.Add(temp);
        }

        // add the curated list to the main personListVMList
        PersonListVM curatedListVM = new PersonListVM(curatedList);
        personListVMList.Add(curatedListVM);

        // return the main list that holds the View Model lists
        return View("Index", personListVMList);
    }


    // [HttpPost]
    // public IActionResult AddList(int userId, int listTypeId, string listName)
    // {
    //     Person person = _personRepository.FindById(userId);
    //     List<PersonGameList> gameLists = _personGameListRepository.GetAll()
    //                                                                 .Where(l => l.PersonId == userId)
    //                                                                 .ToList();
    //     UserListsViewModel userVM = new UserListsViewModel(person, gameLists);

    //     if (person == null)
    //     {
    //         return View("Error");
    //     }

    //     //! listTypeId == 4 means the list is a custom list.
    //     if (listTypeId != 4 && listName != null)
    //     {
    //         gameLists = _personGameListRepository.GetAll()
    //                                             .Where(l => l.PersonId == userId)
    //                                             .ToList();
    //         userVM = new UserListsViewModel(person, gameLists);
    //         ViewBag.ErrorMessage = $"A non custom list may not have a custom name!";
    //         return View("Index", userVM);
    //     }

    //     // *check if the user already has a default list
    //     if (listTypeId != 4)
    //     {
    //         try
    //         {
    //             bool check = _personGameListRepository.CheckIfUserHasDefaultListAlready(person, listTypeId);
    //             if (!check)
    //             {
    //                 gameLists = _personGameListRepository.GetAll()
    //                                                 .Where(l => l.PersonId == userId)
    //                                                 .ToList();
    //                 userVM = new UserListsViewModel(person, gameLists);
    //                 // * listTypeId- 1 because indexes are base zero
    //                 ViewBag.ErrorMessage = $"You already have a {listTypes[listTypeId - 1]} List!";
    //                 return View("Index", userVM);
    //             }
    //             GamePlayListType gamePlayListType = _gamePlayListType.FindById(listTypeId);
    //             ListName defaultListName = _listNameRepository.FindById(listTypeId);
    //             _personGameListRepository.AddList(person, gamePlayListType, defaultListName);
    //             gameLists = _personGameListRepository.GetAll()
    //                                                 .Where(l => l.PersonId == userId)
    //                                                 .ToList();
    //             userVM = new UserListsViewModel(person, gameLists);
    //         }
    //         catch (Exception e)
    //         {
    //             Debug.WriteLine(e);
    //             ViewBag.ErrorMessage = "Something went wrong, please try again.";
    //             return View("Index", userVM);
    //         }

    //     }

    //     if (listTypeId == 4)
    //     {
    //         if (listName == null || listName.Length == 0)
    //         {
    //             ViewBag.ErrorMessage = $"A list name cannot be empty!!!";
    //             return View("Index", userVM);
    //         }
    //         var check = _personGameListRepository.CheckIfUserHasCustomListWithSameName(person, listName);
    //         if (check)
    //         {
    //             gameLists = _personGameListRepository.GetAll()
    //                                             .Where(l => l.PersonId == userId)
    //                                             .ToList();
    //             userVM = new UserListsViewModel(person, gameLists);
    //             ViewBag.ErrorMessage = $"A list with the name {listName} already exists, try a different one!";
    //             return View("Index", userVM);
    //         }
    //         else
    //         {
    //             try
    //             {
    //                 GamePlayListType gamePlayListType = _gamePlayListType.FindById(listTypeId);
    //                 ListName listNameObj = new ListName { NameOfList = listName };
    //                 listNameObj = _listNameRepository.AddOrUpdate(listNameObj);
    //                 _personGameListRepository.AddList(person, gamePlayListType, listNameObj);
    //                 person = _personRepository.FindById(userId);
    //                 gameLists = _personGameListRepository.GetAll()
    //                                                     .Where(l => l.PersonId == userId)
    //                                                     .ToList();
    //                 userVM = new UserListsViewModel(person, gameLists);
    //             }
    //             catch (Exception e)
    //             {
    //                 Debug.WriteLine(e);
    //                 ViewBag.ErrorMessage = $"Something went wrong, please try again.";
    //                 return View("Index", userVM);
    //             }
    //         }
    //     }
    //     ViewBag.Message = "Success!";
    //     return View("Index", userVM);
    // }

    // [HttpPost]
    // public IActionResult DeleteList(int userId, string listName)
    // {
    //     var loggedInUser = _personRepository.GetAll()
    //                                          .Where(user => user.AuthorizationId == _userManager.GetUserId(User))
    //                                          .First();
    //     var usersLists = _personGameListRepository.GetAll()
    //                                              .Where(lists => lists.PersonId == loggedInUser.Id)
    //                                              .ToList();
    //     UserListsViewModel userVM = new UserListsViewModel(loggedInUser, usersLists);
    //     //* make sure the input list name is not null
    //     if (listName == null)
    //     {
    //         ViewBag.ErrorMessage = $"A list name cannot be empty!!!";
    //         return View("Index", userVM);
    //     }
    //     // * check if the list exists
    //     try
    //     {
    //         PersonGameList list = _personGameListRepository.GetAll()
    //                                                         .Where(l => l.PersonId == userId &&
    //                                                         l.ListName.NameOfList == listName)
    //                                                         .First();
    //     }
    //     catch (Exception e)
    //     {
    //         loggedInUser = _personRepository.GetAll()
    //                                          .Where(user => user.AuthorizationId == _userManager.GetUserId(User))
    //                                          .First();
    //         usersLists = _personGameListRepository.GetAll()
    //                                                 .Where(lists => lists.PersonId == loggedInUser.Id)
    //                                                 .ToList();
    //         userVM = new UserListsViewModel(loggedInUser, usersLists);
    //         Debug.WriteLine(e);
    //         ViewBag.ErrorMessage = $"You do not have a list called {listName}";
    //         return View("Index", userVM);
    //     }

    //     // *Prevent deletion of a default list
    //     try
    //     {
    //         var list = _personGameListRepository.GetAll()
    //                                             .Where(l => l.PersonId == userId &&
    //                                             l.ListName.NameOfList == listName)
    //                                             .First();
    //         if (list.ListKindId != 4)
    //         {
    //             ViewBag.ErrorMessage = "You may not delete a default list.";
    //             return View("Index", userVM);
    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.WriteLine(e);
    //         ViewBag.ErrorMessage = "Something went wrong. Please try again.";
    //         return View("Index", userVM);
    //     }

    //     // * if we gotten to this point, the list can be deleted.
    //     try
    //     {
    //         // * create a list of the items inside the list to be deleted.
    //         List<PersonGameList> listToDelete = _personGameListRepository.GetAll()
    //                                                     .Where(l => l.PersonId == userId
    //                                                     && l.ListName.NameOfList == listName)
    //                                                     .ToList();
    //         _personGameListRepository.DeleteACustomList(listToDelete);

    //     }
    //     catch (Exception e)
    //     {
    //         Debug.WriteLine(e);
    //         ViewBag.ErrorMessage = $"Something went wrong when trying to delete list {listName}. Please try again.";
    //     }
    //     //* update the view model.
    //     loggedInUser = _personRepository.GetAll()
    //                                     .Where(user => user.AuthorizationId == _userManager.GetUserId(User))
    //                                     .First();
    //     usersLists = _personGameListRepository.GetAll()
    //                                             .Where(lists => lists.PersonId == loggedInUser.Id)
    //                                             .ToList();
    //     userVM = new UserListsViewModel(loggedInUser, usersLists);


    //     ViewBag.Message = $"Deletion of {listName} successful!";
    //     return View("Index", userVM);
    // }
}