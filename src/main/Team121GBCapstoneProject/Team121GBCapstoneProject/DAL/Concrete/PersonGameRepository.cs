using System.Diagnostics;
using Team121GBCapstoneProject.DAL.Abstract;
using Team121GBCapstoneProject.Models;

namespace Team121GBCapstoneProject.DAL.Concrete;

public class PersonGameRepository : Repository<PersonGame>, IPersonGameRepository
{
    public PersonGameRepository(GPDbContext ctx) : base(ctx)
    {
    }

    // ** This is not being used right now **
    public PersonGame RemovePersonGame(Person person, PersonList personList, PersonGame personGame)
    {

        Delete(personGame);
        return personGame;
    }
}


