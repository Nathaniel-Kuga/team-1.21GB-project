using Team121GBCapstoneProject.Models;

namespace Team121GBCapstoneProject.DAL.Abstract;
public interface IPersonRepository : IRepository<Person>
{
    public bool AddPersonToProjectDb(string authorizationID);
    // public bool CheckIfUserHasCustomListWithSameName (Person user, string listName);
    // public bool CheckIfUserHasDefaultListAlready (Person user, int listType);
    // public bool AddDefaultList(Person user, int listType, string listName);
    // public bool AddCustomList(Person user, int listType, string listName);

}
