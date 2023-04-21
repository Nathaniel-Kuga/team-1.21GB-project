using Team121GBCapstoneProject.Models;
namespace Team121GBCapstoneProject.DAL.Abstract;

public interface IPersonListRepository : IRepository<PersonList>
{
    public bool AddDefaultListsOnAccountCreation(Person person, List<ListKind> listKinds);
}
