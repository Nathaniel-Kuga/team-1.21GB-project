using System.Diagnostics;
using Team121GBCapstoneProject.DAL.Abstract;
using Team121GBCapstoneProject.Models;

namespace Team121GBCapstoneProject.DAL.Concrete;

public class PersonListRepository : Repository<PersonList>, IPersonListRepository
{
    public PersonListRepository(GPDbContext ctx) : base(ctx)
    {
    }

    public bool AddDefaultListsOnAccountCreation(Person person, List<ListKind> listKinds)
    {
        try
        {
            List<PersonList> personList = new List<PersonList>();
            foreach (var listKind in listKinds)
            {
                personList.Add(new PersonList
                {
                    ListKind = listKind.Kind,
                    ListKindId = listKind.Id,
                    PersonId = person.Id
                });
            }
            foreach (var list in personList)
            {
                AddOrUpdate(list);
            }
            return true;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return false;
        }
    }
}

