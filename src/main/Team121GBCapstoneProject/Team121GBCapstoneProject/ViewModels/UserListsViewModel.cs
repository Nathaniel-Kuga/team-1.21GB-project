using Microsoft.AspNetCore.Mvc.Rendering;
using Team121GBCapstoneProject.Models;

namespace Team121GBCapstoneProject.ViewModels;

public class UserListsViewModel
{
    public Person LoggedInUser { get; set; }
    /*
    wanted to make it so if a user already has their default lists,
     they can't see them in the select list in the view but 
    I couln't figure out how to do this so I left it for later.
    */
    //public List<SelectListItem> SelectListItems { get; set; } 
    public List<string> ListNames { get; set; }
    public List<List<PersonGameList>> UsersLists { get; set; }
    public UserListsViewModel() { }
    public UserListsViewModel(Person user, List<PersonGameList> userGames)
    {
        LoggedInUser = user;
        ListNames = userGames.Select(listName => listName.ListName.NameOfList)
                             .Distinct()
                             .ToList();
        UsersLists = userGames.OrderBy(listName => listName.ListNameId)
                              .GroupBy(listNameId => listNameId.ListNameId)
                              .Select(group => group.ToList())
                              .ToList();
    }
}