﻿using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium.Interactions;
using Team121GBCapstoneProject.Areas.Identity.Data;
using Team121GBCapstoneProject.DAL.Concrete;
using Team121GBCapstoneProject.DAL.Abstract;
using Team121GBCapstoneProject.Models;

namespace Team121GBCapstoneProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IGameRepository _gameRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPersonRepository _personRepository;
    private readonly IPersonListRepository _personListRepository;
    private readonly IRepository<PersonGame> _personGameRepository;

    public HomeController(ILogger<HomeController> logger, IGameRepository gameRepo, UserManager<ApplicationUser> userManager, 
        IPersonRepository personRepository, IPersonListRepository personListRepository, IRepository<PersonGame> personGameRepository)
    {
        _logger = logger;
        _gameRepository = gameRepo;
        _userManager = userManager;
        _personRepository = personRepository;
        _personListRepository = personListRepository;
        _personGameRepository = personGameRepository;
    }

    public IActionResult Index()
    {
        GameInfo gameList = new GameInfo();
        gameList.games = _gameRepository.GetTrendingGames(10);
        return View("Index", gameList);
    }

    public IActionResult Index1()
    {
        GameInfo gameList = new GameInfo();
        gameList.games = _gameRepository.GetTrendingGames(10);
        return View("Index1", gameList);
    }

    [Authorize]
    public IActionResult GenerateImage()
    {
        return View();
    }

    [Authorize]
    public IActionResult FindFriends()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public IActionResult FindFriends(string email)
    {
        ApplicationUser foundUser = _userManager.FindByEmailAsync(email).Result;
        FindFriendsVM friendVM = new FindFriendsVM();

        if (foundUser != null)
        {
            friendVM.User = foundUser;
            PersonList personList = _personListRepository.GetAll().FirstOrDefault(pl =>
                pl.ListKind == "Currently Playing" && pl.Person.AuthorizationId == foundUser.Id);
            friendVM.Games = _personGameRepository.GetAll().Where(gl => 
                gl.PersonListId == personList.Id && gl.PersonList.Person.AuthorizationId == foundUser.Id).ToList();
        }
        else
        {
            friendVM.PersonNotFound = true;
        }
        
        return View("FindFriends", friendVM);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
