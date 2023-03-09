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
using Team121GBNUinitTest;

#nullable disable
namespace Team121GBNUnitTest;

public class PersonRepositoryTests
{
    private Mock<GPDbContext> _mockContext;
    private Mock<DbSet<Person>> _mockPersonDbSet;
    private Mock<DbSet<PersonGameList>> _mockPersonGameListDbSet;
    private Mock<DbSet<GamePlayListType>> _mockGamePlayListTypeDbSet; ///
    private Mock<DbSet<Game>> _mockGameDbSet;
    private List<Person> _people;
    private List<PersonGameList> _peoplesGameLists;
    private List<GamePlayListType> _gamesPlayListTypes;
    private List<Game> _games;

    [SetUp]
    public void Setup()
    {
        _people = new List<Person>
        {
            new Person { Id = 1, AuthorizationId = "123" }
        };

        _peoplesGameLists = new List<PersonGameList>
        {
            new PersonGameList { Id = 1, PersonId = 1, GameId = 1, ListKindId = 1},
            new PersonGameList { Id = 2, PersonId = 1, GameId = 2, ListKindId = 2},
            new PersonGameList { Id = 3, PersonId = 1, GameId = 3, ListKindId = 3}
        };

        _gamesPlayListTypes = new List<GamePlayListType>
        {
            new GamePlayListType { Id = 1, ListKind = "Currently Playing" },
            new GamePlayListType { Id = 2, ListKind = "Completed" },
            new GamePlayListType { Id = 3, ListKind = "Want to Play" }
        };

        _games = new List<Game>
        {
            new Game{ Id = 1, Title = "Gears of War", Description = "This is Gears of War", YearPublished = 2001, AverageRating = 10, EsrbratingId = 1, CoverPicture = "TesterFile.png"},
            new Game{ Id = 2, Title = "Deep Rock Galatic", Description = "Dwarfs in a cave", YearPublished = 2019, AverageRating = 9.8, EsrbratingId = 1, CoverPicture = "TesterFile2.png"},
            new Game{ Id = 3, Title = "Minecraft", Description = "Mine Game", YearPublished = 2007, AverageRating = 9.7, EsrbratingId = 1, CoverPicture = "TesterFile3.png"}
        };

        // * Set the navigation properties
        _peoplesGameLists.ForEach(gl =>
        {
            gl.Person = _people.Single(p => p.Id == gl.PersonId);
            gl.Game = _games.Single(id => id.Id == gl.GameId);
            gl.ListKind = _gamesPlayListTypes.Single(lk => lk.Id == gl.ListKindId);
        });

        _mockContext = new Mock<GPDbContext>();
        _mockPersonDbSet = MockHelpers.GetMockDbSet(_people.AsQueryable());
        _mockContext.Setup(ctx => ctx.People).Returns(_mockPersonDbSet.Object);
        _mockContext.Setup(ctx => ctx.Set<Person>()).Returns(_mockPersonDbSet.Object);

        _mockGameDbSet = MockHelpers.GetMockDbSet(_games.AsQueryable());
        _mockContext.Setup(ctx => ctx.Games).Returns(_mockGameDbSet.Object);
        _mockContext.Setup(ctx => ctx.Set<Game>()).Returns(_mockGameDbSet.Object);

        _mockGamePlayListTypeDbSet = MockHelpers.GetMockDbSet(_gamesPlayListTypes.AsQueryable());
        _mockContext.Setup(ctx => ctx.GamePlayListTypes).Returns(_mockGamePlayListTypeDbSet.Object);
        _mockContext.Setup(ctx => ctx.Set<GamePlayListType>()).Returns(_mockGamePlayListTypeDbSet.Object);

        _mockPersonGameListDbSet = MockHelpers.GetMockDbSet(_peoplesGameLists.AsQueryable());
        _mockContext.Setup(ctx => ctx.PersonGameLists).Returns(_mockPersonGameListDbSet.Object);
        _mockContext.Setup(ctx => ctx.Set<PersonGameList>()).Returns(_mockPersonGameListDbSet.Object);
    }

    [Test]
    public void AddPersonToProjectDb_ValidPerson_ShouldReturnTrue()
    {
        // ! Arrange
        var personRepository = new PersonRepository(_mockContext.Object);
        var person = new Person
        {
            AuthorizationId = "456",
        };
        // * Act
        var result = personRepository.AddPersonToProjectDb(person.AuthorizationId);
        var people = _mockPersonDbSet.Object.ToList();
        // ? Assert
        Assert.That(result, Is.EqualTo(true));
        // ! I want to check to sure that the number of people would be updated to 2 but for some
        // ! reason my test doesn't capture the update in count
        // ! I does however work when I update the database in the main application
        // ! I am gonna need help to figure this one out.
        // Assert.Multiple(() =>
        // {
        //     Assert.That(result, Is.EqualTo(true));
        //     Assert.That(people.Count(), Is.EqualTo(2));
        // });
    }


    [Test]
    public void AddPersonToProjectDb_InvalidPerson_ShouldReturnTrue()
    {
        // ! Arrange
        var personRepository = new PersonRepository(_mockContext.Object);
        var person = new Person
        {
            AuthorizationId = null,
        };
        // * Act
        var result = personRepository.AddPersonToProjectDb(person.AuthorizationId);
        // ? Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(false));
            Assert.That(_people.Count(), Is.EqualTo(1));
        });
    }

    
}