using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Team121GBCapstoneProject.DAL.Abstract;
using Team121GBCapstoneProject.DAL.Concrete;
using Team121GBCapstoneProject.Models;

namespace Team121GBNUnitTest;

/**
 * This is the recommended way to test using the in-memory db.  Seed the db and then write your tests based only on the seed
 * data + anything else you do to it.  No other tests will modify the db for that test.  Every test gets a brand new seeded db.
 * 
 */
public class PersonRepositoryTests
{
    // ! I misunderstood where this path leads to. I thought it was supposed to go to the main project Data folder, 
    // ! but it turns out that the path is for seed file you put into the Data folder inside this project.
    private static readonly string _seedFile = System.IO.Path.Combine("..", "..", "..", "Data", "seed.sql");/*@"..\..\..\Data\seed.sql";*/ // relative path from where the executable is: bin/Debug/net7.0
    // Create this helper like this, for whatever context you desire
    private InMemoryDbHelper<GPDbContext> _dbHelper = new InMemoryDbHelper<GPDbContext>(_seedFile, DbPersistence.OneDbPerTest);

    [Test]
    public void GPDbContext_AddPersonTpProjectDb_Success_ShouldReturnTrue()
    {   
        // * Arrange 
        using GPDbContext context = _dbHelper.GetContext();
        PersonRepository personRepository = new PersonRepository(context);
        string authorizationId = "some-String-123";

        // ! Act
        bool result = personRepository.AddPersonToProjectDb(authorizationId);
        int numberOfPeopleInDb = personRepository.GetAll().Count();

        // ? Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(true));
            Assert.That(numberOfPeopleInDb, Is.EqualTo(1));
        });
    }
    [Test]
    public void GPDbContext_AddPersonTpProjectDb_NullAuthorizationId_ShouldReturnFalse()
    {
        // * Arrange 
        using GPDbContext context = _dbHelper.GetContext();
        PersonRepository personRepository = new PersonRepository(context);
        string authorizationId = null;

        // ! Act
        bool result = personRepository.AddPersonToProjectDb(authorizationId);
        int numberOfPeopleInDb = personRepository.GetAll().Count();

        // ? Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(false));
            Assert.That(numberOfPeopleInDb, Is.EqualTo(0));
        });
    }
    [Test]
    public void GPDbContext_AddPersonTpProjectDb_EmptyAuthorizationIdString_ShouldReturnFalse()
    {
        // * Arrange 
        using GPDbContext context = _dbHelper.GetContext();
        PersonRepository personRepository = new PersonRepository(context);
        string authorizationId = "";

        // ! Act
        bool result = personRepository.AddPersonToProjectDb(authorizationId);
        int numberOfPeopleInDb = personRepository.GetAll().Count();

        // ? Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(false));
            Assert.That(numberOfPeopleInDb, Is.EqualTo(0));
        });
    }
}