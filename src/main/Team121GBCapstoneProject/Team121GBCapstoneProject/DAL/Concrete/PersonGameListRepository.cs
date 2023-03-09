using Team121GBCapstoneProject.Models;
using Team121GBCapstoneProject.DAL.Abstract;
using System.Linq.Expressions;
using System.Diagnostics;

namespace Team121GBCapstoneProject.DAL.Concrete;

public class PersonGameListRepository : Repository<PersonGameList>, IPersonGameListRepository
{
    public PersonGameListRepository(GPDbContext ctx) : base(ctx)
    {

    }
    public bool CheckIfUserHasCustomListWithSameName(Person user, string listName)
    {
        bool check = false;
        List<PersonGameList> listNames = user.PersonGameLists.ToList();

        // ! check if a list of the same name exists for user.
        foreach (var list in listNames)
        {
            if (list.ListName.NameOfList == listName)
            {
                check = true;
                return check;
            }
        }
        return check;
    }
    public bool CheckIfUserHasDefaultListAlready(Person user, int listType)
    {
        bool check = false;
        List<PersonGameList> listTypes = user.PersonGameLists.ToList();
        foreach (var list in listTypes)
        {
            if (list.ListKindId == listType) return check;
        }
        check = true;
        return check;
    }
    public void AddList(Person user, GamePlayListType listType, ListName listName)
    {
        PersonGameList newList = new PersonGameList
        {
            Person = user,
            PersonId = user.Id,
            ListKind = listType,
            ListKindId = listType.Id,
            ListName = listName,
            ListNameId = listName.Id
        };
        AddOrUpdate(newList);
        
    }
    
    public void DeleteACustomList(List<PersonGameList> listToDelete)
    {
        foreach (var listItem in listToDelete)
        {
            DeleteById(listItem.Id);
        }
    }
}