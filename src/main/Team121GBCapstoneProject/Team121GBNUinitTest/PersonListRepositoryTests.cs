using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Team121GBCapstoneProject.DAL.Abstract;
using Team121GBCapstoneProject.DAL.Concrete;
using Team121GBCapstoneProject.Models;

namespace Team121GBNUnitTest;

public class PersonListRepositoryTests
{
    private static readonly string _seedFile = @"..\..\..\Data\seed.sql"; // relative path from where the executable is: bin/Debug/net7.0
    // Create this helper like this, for whatever context you desire
    private InMemoryDbHelper<GPDbContext> _dbHelper = new InMemoryDbHelper<GPDbContext>(_seedFile, DbPersistence.OneDbPerTest);

    [Test]
    public void GPDbContext_CreateDefaultLists_Success_ShouldReturnTrue()
    {
        // * Arrange 
        using GPDbContext context = _dbHelper.GetContext();
        PersonListRepository personListRepository = new PersonListRepository(context);
        PersonRepository personRepository = new PersonRepository(context);
        string authorizationId = "some-String";
        ListKindRepository listKindRepository = new ListKindRepository(context);
        List<ListKind> listKinds = listKindRepository.GetAll().ToList();
        //add a valid person to db
        personRepository.AddPersonToProjectDb(authorizationId);
        //Get the person that was added to db and pass it to default list method
        Person person = personRepository.GetAll().FirstOrDefault(p => p.AuthorizationId == authorizationId);

        // ! Act
        bool result = personListRepository.AddDefaultListsOnAccountCreation(person, listKinds);
        int count = personListRepository.GetAll().Count();
        List<PersonList> personListsInDb = personListRepository.GetAll().ToList();
        // ? Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(true));
            Assert.That(count, Is.EqualTo(3));
        });
    }

    [Test]
    public void GPDbContext_CreateDefaultLists_Failure_PersonIsNull_ShouldReturnFalse()
    {
        // * Arrange 
        using GPDbContext context = _dbHelper.GetContext();
        PersonListRepository personListRepository = new PersonListRepository(context);
        ListKindRepository listKindRepository = new ListKindRepository(context);
        List<ListKind> listKinds = listKindRepository.GetAll().ToList();
        Person person = null;
        // ! Act
        bool result = personListRepository.AddDefaultListsOnAccountCreation(person, listKinds);
        int count = personListRepository.GetAll().Count();
        // ? Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(false));
            Assert.That(count, Is.EqualTo(0));
        });

    }
    [Test]
    public void GPDbContext_CreateDefaultLists_Failure_ListNameIsNull_ShouldReturnFalse()
    {
        // * Arrange 
        using GPDbContext context = _dbHelper.GetContext();
        PersonListRepository personListRepository = new PersonListRepository(context);
        ListKindRepository listKindRepository = new ListKindRepository(context);
        List<ListKind> listKinds = null;
        PersonRepository personRepository = new PersonRepository(context);
        string authorizationId = "some-String";
        //add a valid person to db
        personRepository.AddPersonToProjectDb(authorizationId);
        //Get the person that was added to db and pass it to default list method
        Person person = personRepository.GetAll().FirstOrDefault(p => p.AuthorizationId == authorizationId);

        // ! Act
        bool result = personListRepository.AddDefaultListsOnAccountCreation(person, listKinds);
        int count = personListRepository.GetAll().Count();
        // ? Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(false));
            Assert.That(count, Is.EqualTo(0));
        });

    }
}

