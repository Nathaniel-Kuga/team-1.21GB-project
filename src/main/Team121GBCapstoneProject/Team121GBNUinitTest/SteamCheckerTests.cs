using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Team121GBCapstoneProject.Models;
using Team121GBCapstoneProject.Services.Abstract;
using Team121GBCapstoneProject.Services.Concrete;

namespace Team121GBNUnitTest
{
    public class SteamCheckerTests
    {
       /*       private Mock<GPDbContext> _mockContext;
              private Mock<DbSet<PersonGame>> _mockGameDbSet;
              private List<PersonGame> _game;

              [SetUp]
              public void Setup()
              {
                  _game = new List<PersonGame>
                  {
                      new PersonGame{ Id = 1, PersonListId=3 ,GameId=103},
                  };

                  _mockContext = new Mock<GPDbContext>();
                  _mockGameDbSet = MockHelpers.GetMockDbSet(_game.AsQueryable());
                  _mockContext.Setup(ctx => ctx.PersonGames).Returns(_mockGameDbSet.Object);
                  _mockContext.Setup(ctx => ctx.Set<PersonGame>()).Returns(_mockGameDbSet.Object);
              }*/

        //Set up In memory db

        [Test]
        public void checkGameTitleWithMatchingGamesShouldReturnTrue()
        {
            List<PersonGame> pgs = new List<PersonGame>
            {
                new PersonGame
                {
                    Id = 1,
                    GameId = 1,
                    PersonListId = 1,
                    Game = new Game
                    {
                        Id = 1,
                        Title = "Diablo",
                        Description = "Description",
                        YearPublished = 1996,
                        EsrbratingId = 6,
                        AverageRating = 1,
                        CoverPicture = "",
                        Igdburl = "",
                        IgdbgameId = 1
                    }
                }
            };

            ISteamChecker steamChecker = new SteamChecker();

            //Arrange
            bool result = steamChecker.checkGameTitle("Diablo", pgs);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void checkGameTitleWithNoMatchingGamesShouldReturnFalse()
        {
            List<PersonGame> pgs = new List<PersonGame>
            {
                new PersonGame
                {
                    Id = 1,
                    GameId = 1,
                    PersonListId = 1,
                    Game = new Game
                    {
                        Id = 1,
                        Title = "Mario Sunshine",
                        Description = "Description",
                        YearPublished = 1996,
                        EsrbratingId = 6,
                        AverageRating = 1,
                        CoverPicture = "",
                        Igdburl = "",
                        IgdbgameId = 1
                    }
                }
            };

            ISteamChecker steamChecker = new SteamChecker();

            //Arrange
            bool result = steamChecker.checkGameTitle("Diablo", pgs);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void checkGameTitleWith2GamesWithMatchingGameShouldReturnTrue()
        {
            List<PersonGame> pgs = new List<PersonGame>
            {
                new PersonGame
                {
                    Id = 1,
                    GameId = 1,
                    PersonListId = 1,
                    Game = new Game
                    {
                        Id = 1,
                        Title = "Mario Sunshine",
                        Description = "Description",
                        YearPublished = 1996,
                        EsrbratingId = 6,
                        AverageRating = 1,
                        CoverPicture = "",
                        Igdburl = "",
                        IgdbgameId = 1
                    }
                },
                new PersonGame
                {
                    Id = 2,
                    GameId = 2,
                    PersonListId = 1,
                    Game = new Game
                    {
                        Id = 2,
                        Title = "Diablo",
                        Description = "Description",
                        YearPublished = 1996,
                        EsrbratingId = 6,
                        AverageRating = 1,
                        CoverPicture = "",
                        Igdburl = "",
                        IgdbgameId = 1
                    }
                }
            };

            ISteamChecker steamChecker = new SteamChecker();

            //Arrange
            bool result = steamChecker.checkGameTitle("Diablo", pgs);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void checkGameTitleWith2GamesWithNoMatchingGameShouldReturnFalse()
        {
            List<PersonGame> pgs = new List<PersonGame>
            {
                new PersonGame
                {
                    Id = 1,
                    GameId = 1,
                    PersonListId = 1,
                    Game = new Game
                    {
                        Id = 1,
                        Title = "Mario Sunshine",
                        Description = "Description",
                        YearPublished = 1996,
                        EsrbratingId = 6,
                        AverageRating = 1,
                        CoverPicture = "",
                        Igdburl = "",
                        IgdbgameId = 1
                    }
                },
                new PersonGame
                {
                    Id = 2,
                    GameId = 2,
                    PersonListId = 1,
                    Game = new Game
                    {
                        Id = 2,
                        Title = "Zelda",
                        Description = "Description",
                        YearPublished = 1996,
                        EsrbratingId = 6,
                        AverageRating = 1,
                        CoverPicture = "",
                        Igdburl = "",
                        IgdbgameId = 1
                    }
                }
            };

            ISteamChecker steamChecker = new SteamChecker();

            //Arrange
            bool result = steamChecker.checkGameTitle("Diablo", pgs);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void checkGameIdWithMatchingGamesShouldReturnTrue()
        {
            List<PersonGame> pgs = new List<PersonGame>
            {
                new PersonGame
                {
                    Id = 1,
                    GameId = 1,
                    PersonListId = 1,
                    Game = new Game
                    {
                        Id = 1,
                        Title = "Diablo",
                        Description = "Description",
                        YearPublished = 1996,
                        EsrbratingId = 6,
                        AverageRating = 1,
                        CoverPicture = "",
                        Igdburl = "",
                        IgdbgameId = 1
                    }
                }
            };

            ISteamChecker steamChecker = new SteamChecker();

            //Arrange
            bool result = steamChecker.checkGameId(1, pgs);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void checkGameIdWithNoMatchingGamesShouldReturnFalse()
        {
            List<PersonGame> pgs = new List<PersonGame>
            {
                new PersonGame
                {
                    Id = 1,
                    GameId = 1,
                    PersonListId = 1,
                    Game = new Game
                    {
                        Id = 1,
                        Title = "Diablo",
                        Description = "Description",
                        YearPublished = 1996,
                        EsrbratingId = 6,
                        AverageRating = 1,
                        CoverPicture = "",
                        Igdburl = "",
                        IgdbgameId = 1
                    }
                }
            };

            ISteamChecker steamChecker = new SteamChecker();

            //Arrange
            bool result = steamChecker.checkGameId(12, pgs);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void checkGameIdWith2GamesWithMatchingGameShouldReturnTrue()
        {
            List<PersonGame> pgs = new List<PersonGame>
            {
                new PersonGame
                {
                    Id = 1,
                    GameId = 1,
                    PersonListId = 1,
                    Game = new Game
                    {
                        Id = 1,
                        Title = "Mario Sunshine",
                        Description = "Description",
                        YearPublished = 1996,
                        EsrbratingId = 6,
                        AverageRating = 1,
                        CoverPicture = "",
                        Igdburl = "",
                        IgdbgameId = 1
                    }
                },
                new PersonGame
                {
                    Id = 2,
                    GameId = 2,
                    PersonListId = 1,
                    Game = new Game
                    {
                        Id = 2,
                        Title = "Diablo",
                        Description = "Description",
                        YearPublished = 1996,
                        EsrbratingId = 6,
                        AverageRating = 1,
                        CoverPicture = "",
                        Igdburl = "",
                        IgdbgameId = 1
                    }
                }
            };

            ISteamChecker steamChecker = new SteamChecker();

            //Arrange
            bool result = steamChecker.checkGameId(2, pgs);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void checkGameIdWith2GamesWithNoMatchingGameShouldReturnFalse()
        {
            List<PersonGame> pgs = new List<PersonGame>
            {
                new PersonGame
                {
                    Id = 1,
                    GameId = 1,
                    PersonListId = 1,
                    Game = new Game
                    {
                        Id = 1,
                        Title = "Mario Sunshine",
                        Description = "Description",
                        YearPublished = 1996,
                        EsrbratingId = 6,
                        AverageRating = 1,
                        CoverPicture = "",
                        Igdburl = "",
                        IgdbgameId = 1
                    }
                },
                new PersonGame
                {
                    Id = 2,
                    GameId = 2,
                    PersonListId = 1,
                    Game = new Game
                    {
                        Id = 2,
                        Title = "Zelda",
                        Description = "Description",
                        YearPublished = 1996,
                        EsrbratingId = 6,
                        AverageRating = 1,
                        CoverPicture = "",
                        Igdburl = "",
                        IgdbgameId = 1
                    }
                }
            };

            ISteamChecker steamChecker = new SteamChecker();

            //Arrange
            bool result = steamChecker.checkGameId(12, pgs);

            //Assert
            Assert.IsFalse(result);
        }
    }
}
