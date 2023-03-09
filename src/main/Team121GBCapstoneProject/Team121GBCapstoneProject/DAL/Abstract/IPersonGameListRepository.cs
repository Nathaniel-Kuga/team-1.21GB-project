using Team121GBCapstoneProject.Models;

namespace Team121GBCapstoneProject.DAL.Abstract;
public interface IPersonGameListRepository : IRepository<PersonGameList>
{
    public bool CheckIfUserHasCustomListWithSameName (Person user, string listName);
    public bool CheckIfUserHasDefaultListAlready (Person user, int listType);
    public void AddList(Person user, GamePlayListType listType, ListName listName);
    public void DeleteACustomList(List<PersonGameList> listToDelete);


}
