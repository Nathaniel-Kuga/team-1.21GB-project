using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using Team121GBCapstoneProject.DAL.Abstract;
using Team121GBCapstoneProject.Models;

namespace Team121GBCapstoneProject.DAL.Concrete;

public class PersonRepository : Repository<Person>, IPersonRepository
{
    public PersonRepository(GPDbContext ctx) : base(ctx)
    {

    }

    public bool AddPersonToProjectDb(string authorizationID)
    {
        if (string.IsNullOrEmpty(authorizationID))
        {
            return false;
        }
        else
        {
            try
            {
                Person GPPerson = new Person
                {
                    AuthorizationId = authorizationID
                };
                GPPerson = AddOrUpdate(GPPerson);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return false;
            }
        }
    }
}