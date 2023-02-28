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

namespace Team121GBNUinitTest
{
    public class GameRepositoryTests
    {
        private Mock<GPDbContext> _mockContext;
        private Mock<DbSet<Game>> _mockGameDbSet;
        private List<Game> _game;

        [SetUp]
        public void Setup()
        {
            _game = new List<Game>
            {
                new Game{ Id = 1, Title = "Gears of War", Description = "This is Gears of War", YearPublished = 2001, AverageRating = 10, EsrbratingId = 1, CoverPicture = "TesterFile.png"},
                new Game{ Id = 2, Title = "Deep Rock Galatic", Description = "Dwarfs in a cave", YearPublished = 2019, AverageRating = 9.8, EsrbratingId = 1, CoverPicture = "TesterFile2.png"},
                new Game{ Id = 3, Title = "Minecraft", Description = "Mine Game", YearPublished = 2007, AverageRating = 9.7, EsrbratingId = 1, CoverPicture = "TesterFile3.png"},
                new Game{ Id = 4, Title = "Xenoblade Cronicles", Description = "Too Many Spoliers", YearPublished = 2010, AverageRating = 9.6, EsrbratingId = 1, CoverPicture = "TesterFile4.png"},
                new Game{ Id = 5, Title = "Octopath Traveler", Description = "RPG with a lot of paths", YearPublished = 2020, AverageRating = 8.5, EsrbratingId = 1, CoverPicture = "TesterFile5.png"},
                new Game{ Id = 6, Title = "Getting Over it", Description = "Hard Game with hard controls", YearPublished = 2017, AverageRating = 5.7, EsrbratingId = 1, CoverPicture = "TesterFile6.png" },
                new Game{ Id = 7, Title = "Dark Souls", Description = "Hard but fun game", YearPublished = 2005, EsrbratingId= 1, AverageRating = 8.5, CoverPicture = "TesterFile7.png"},
                new Game{ Id = 8, Title = "Dark Souls 2", Description = "Worst Game in the series", YearPublished = 2007, EsrbratingId = 1, AverageRating = 4.6, CoverPicture = "TesterFile8.png"},
                new Game{ Id = 9, Title = "Dark Souls 3", Description = "Best Dark Souls Game", YearPublished = 2010, EsrbratingId = 1, AverageRating = 7.9, CoverPicture = "TesterFile9.png"},
                new Game{ Id = 10, Title = "BloodBorne", Description = "Bloodborne with eldritch horrors", YearPublished = 2019, EsrbratingId = 1, AverageRating = 9.5, CoverPicture = "TesterFile10.png"},
                new Game{ Id = 11, Title = "Elden Ring", Description = "Great Game, Perfect Souls game", YearPublished = 2022, EsrbratingId = 1, AverageRating = 9.9, CoverPicture = "TesterFile11.png"}
            };

            _mockContext = new Mock<GPDbContext>();
            _mockGameDbSet = MockHelpers.GetMockDbSet(_game.AsQueryable());
            _mockContext.Setup(ctx => ctx.Games).Returns(_mockGameDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<Game>()).Returns(_mockGameDbSet.Object);
        }

        [Test]
        public void GetFeaturedGamesShouldReturn10Games()
        {
            IGameRepository gameRepository = new GameRepository(_mockContext.Object);
            var expected = 10;

            var listGames = gameRepository.GetTrendingGames(10);

            int gameCount = listGames.Count();

            Assert.That(gameCount, Is.EqualTo(expected));
        }

        [Test]
        public void GetFeaturedGamesWithCorrectOrderingShouldReturnTrue()
        {
            IGameRepository gameRepository = new GameRepository(_mockContext.Object);
            var expected = true;
            var actual = true;

            var listGames = gameRepository.GetTrendingGames(10);

            for (int i = 0; i < 10; i++)
            {
                if (i == 9)
                {
                    break;
                }
                if (listGames[i].AverageRating < listGames[i + 1].AverageRating)
                {
                    actual = false; break;
                }
            }

            Assert.That(actual, Is.EqualTo(expected));
        }

    }
}
