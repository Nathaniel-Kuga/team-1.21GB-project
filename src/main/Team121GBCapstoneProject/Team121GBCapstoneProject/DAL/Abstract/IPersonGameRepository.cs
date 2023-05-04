using Team121GBCapstoneProject.Models;
namespace Team121GBCapstoneProject.DAL.Abstract;

public interface IPersonGameRepository : IRepository<PersonGame>
{
    public PersonGame RemovePersonGame(Person person, PersonList personList, PersonGame personGame);
}