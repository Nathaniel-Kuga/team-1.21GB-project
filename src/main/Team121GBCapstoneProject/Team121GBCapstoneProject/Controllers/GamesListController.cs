using Microsoft.AspNetCore.Mvc;
using Team121GBCapstoneProject.Models;
using Team121GBCapstoneProject.DAL.Abstract;
using Team121GBCapstoneProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Team121GBCapstoneProject.Areas.Identity.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;


namespace Team121GBCapstoneProjects.Controllers;

public class GamesListsController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<GamesListsController> _logger;
    private IPersonRepository _personRepository;
    private IPersonGameListRepository _personGameListRepository;
    private IRepository<ListName> _listNameRepository;
    private IRepository<GamePlayListType> _gamePlayListType;
    private readonly List<string> listTypes = new List<string> { "Currently Playing", "Completed", "Want to Play" };
    public GamesListsController(UserManager<ApplicationUser> userManager,
                                 ILogger<GamesListsController> logger,
                                 IPersonRepository personRepository,
                                 IPersonGameListRepository personGameListRepository,
                                 IRepository<ListName> listNameRepository,
                                 IRepository<GamePlayListType> gamePlayListType)
    {
        _userManager = userManager;
        _logger = logger;
        _personRepository = personRepository;
        _personGameListRepository = personGameListRepository;
        _listNameRepository = listNameRepository;
        _gamePlayListType = gamePlayListType;
    }

    [Authorize]
    [HttpGet]
    public IActionResult Index()
    {
        var loggedInUser = _personRepository.GetAll()
                                               .Where(user => user.AuthorizationId == _userManager.GetUserId(User))
                                               .First();
        var usersLists = _personGameListRepository.GetAll()
                                                 .Where(lists => lists.PersonId == loggedInUser.Id)
                                                 .ToList();
        UserListsViewModel userVM = new UserListsViewModel(loggedInUser, usersLists);
        return View("Index", userVM);
    }

    [HttpPost]
    public IActionResult AddList(int userId, int listTypeId, string listName)
    {
        Person person = _personRepository.FindById(userId);
        List<PersonGameList> gameLists = _personGameListRepository.GetAll()
                                                                    .Where(l => l.PersonId == userId)
                                                                    .ToList();
        UserListsViewModel userVM = new UserListsViewModel(person, gameLists);

        if (person == null)
        {
            return View("Error");
        }

        //! listTypeId == 4 means the list is a custom list.
        if (listTypeId != 4 && listName != null)
        {
            gameLists = _personGameListRepository.GetAll()
                                                .Where(l => l.PersonId == userId)
                                                .ToList();
            userVM = new UserListsViewModel(person, gameLists);
            ViewBag.ErrorMessage = $"A non custom list may not have a custom name!";
            return View("Index", userVM);
        }

        // *check if the user already has a default list
        if (listTypeId != 4)
        {
            try
            {
                bool check = _personGameListRepository.CheckIfUserHasDefaultListAlready(person, listTypeId);
                if (!check)
                {
                    gameLists = _personGameListRepository.GetAll()
                                                    .Where(l => l.PersonId == userId)
                                                    .ToList();
                    userVM = new UserListsViewModel(person, gameLists);
                    // * listTypeId- 1 because indexes are base zero
                    ViewBag.ErrorMessage = $"You already have a {listTypes[listTypeId - 1]} List!";
                    return View("Index", userVM);
                }
                GamePlayListType gamePlayListType = _gamePlayListType.FindById(listTypeId);
                ListName defaultListName = _listNameRepository.FindById(listTypeId);
                _personGameListRepository.AddList(person, gamePlayListType, defaultListName);
                gameLists = _personGameListRepository.GetAll()
                                                    .Where(l => l.PersonId == userId)
                                                    .ToList();
                userVM = new UserListsViewModel(person, gameLists);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                ViewBag.ErrorMessage = "Something went wrong, please try again.";
                return View("Index", userVM);
            }

        }

        if (listTypeId == 4)
        {
            if (listName == null || listName.Length == 0)
            {
                ViewBag.ErrorMessage = $"A list name cannot be empty!!!";
                return View("Index", userVM);
            }
            var check = _personGameListRepository.CheckIfUserHasCustomListWithSameName(person, listName);
            if (check)
            {
                gameLists = _personGameListRepository.GetAll()
                                                .Where(l => l.PersonId == userId)
                                                .ToList();
                userVM = new UserListsViewModel(person, gameLists);
                ViewBag.ErrorMessage = $"A list with the name {listName} already exists, try a different one!";
                return View("Index", userVM);
            }
            else
            {
                try
                {
                    GamePlayListType gamePlayListType = _gamePlayListType.FindById(listTypeId);
                    ListName listNameObj = new ListName { NameOfList = listName };
                    listNameObj = _listNameRepository.AddOrUpdate(listNameObj);
                    _personGameListRepository.AddList(person, gamePlayListType, listNameObj);
                    person = _personRepository.FindById(userId);
                    gameLists = _personGameListRepository.GetAll()
                                                        .Where(l => l.PersonId == userId)
                                                        .ToList();
                    userVM = new UserListsViewModel(person, gameLists);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    ViewBag.ErrorMessage = $"Something went wrong, please try again.";
                    return View("Index", userVM);
                }
            }
        }
        ViewBag.Message = "Success!";
        return View("Index", userVM);
    }

    [HttpPost]
    public IActionResult DeleteList(int userId, string listName)
    {
        var loggedInUser = _personRepository.GetAll()
                                             .Where(user => user.AuthorizationId == _userManager.GetUserId(User))
                                             .First();
        var usersLists = _personGameListRepository.GetAll()
                                                 .Where(lists => lists.PersonId == loggedInUser.Id)
                                                 .ToList();
        UserListsViewModel userVM = new UserListsViewModel(loggedInUser, usersLists);
        //* make sure the input list name is not null
        if (listName == null)
        {
            ViewBag.ErrorMessage = $"A list name cannot be empty!!!";
            return View("Index", userVM);
        }
        // * check if the list exists
        try
        {
            PersonGameList list = _personGameListRepository.GetAll()
                                                            .Where(l => l.PersonId == userId &&
                                                            l.ListName.NameOfList == listName)
                                                            .First();
        }
        catch (Exception e)
        {
            loggedInUser = _personRepository.GetAll()
                                             .Where(user => user.AuthorizationId == _userManager.GetUserId(User))
                                             .First();
            usersLists = _personGameListRepository.GetAll()
                                                    .Where(lists => lists.PersonId == loggedInUser.Id)
                                                    .ToList();
            userVM = new UserListsViewModel(loggedInUser, usersLists);
            Debug.WriteLine(e);
            ViewBag.ErrorMessage = $"You do not have a list called {listName}";
            return View("Index", userVM);
        }

        // *Prevent deletion of a default list
        try
        {
            var list = _personGameListRepository.GetAll()
                                                .Where(l => l.PersonId == userId &&
                                                l.ListName.NameOfList == listName)
                                                .First();
            if (list.ListKindId != 4)
            {
                ViewBag.ErrorMessage = "You may not delete a default list.";
                return View("Index", userVM);
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            ViewBag.ErrorMessage = "Something went wrong. Please try again.";
            return View("Index", userVM);
        }

        // * if we gotten to this point, the list can be deleted.
        try
        {
            // * create a list of the items inside the list to be deleted.
            List<PersonGameList> listToDelete = _personGameListRepository.GetAll()
                                                        .Where(l => l.PersonId == userId
                                                        && l.ListName.NameOfList == listName)
                                                        .ToList();
            _personGameListRepository.DeleteACustomList(listToDelete);

        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            ViewBag.ErrorMessage = $"Something went wrong when trying to delete list {listName}. Please try again.";
        }
        //* update the view model.
        loggedInUser = _personRepository.GetAll()
                                        .Where(user => user.AuthorizationId == _userManager.GetUserId(User))
                                        .First();
        usersLists = _personGameListRepository.GetAll()
                                                .Where(lists => lists.PersonId == loggedInUser.Id)
                                                .ToList();
        userVM = new UserListsViewModel(loggedInUser, usersLists);


        ViewBag.Message = $"Deletion of {listName} successful!";
        return View("Index", userVM);
    }
}