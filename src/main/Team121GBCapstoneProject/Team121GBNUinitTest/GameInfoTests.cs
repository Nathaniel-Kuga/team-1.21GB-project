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

namespace Team121GBNUnitTest
{
    public class GameInfoTests
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
                new Game{ Id = 2, Title = "Deep Rock Galatic", Description = "Dwarfs in a cave", YearPublished = 2019, AverageRating = 7.5, EsrbratingId = 1, CoverPicture = "TesterFile2.png"},
                new Game{ Id = 3, Title = "Minecraft", Description = "Mine Game", YearPublished = 2007, AverageRating = 4.7, EsrbratingId = 1, CoverPicture = "TesterFile3.png"},
                new Game{ Id = 4, Title = "Xenoblade Cronicles", Description = "Too Many Spoliers", YearPublished = 2010, AverageRating = 0.6, EsrbratingId = 1, CoverPicture = "TesterFile4.png"},
            };

            _mockContext = new Mock<GPDbContext>();
            _mockGameDbSet = MockHelpers.GetMockDbSet(_game.AsQueryable());
            _mockContext.Setup(ctx => ctx.Games).Returns(_mockGameDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<Game>()).Returns(_mockGameDbSet.Object);
        }

        [Test]
        public void ColorSelectionTestingAllFourColorsShouldReturnTrue()
        {
            IGameRepository gameRepository = new GameRepository(_mockContext.Object);
            var expected = true;
            var actual = true;
            GameInfo _gameInfo = new GameInfo();

            var featuredGames = gameRepository.GetTrendingGames(4);
            var colorSelection = _gameInfo.colorSelection(featuredGames);

            if (colorSelection[0] != "#49c44d")
            {
                actual = false;
            }
            if (colorSelection[1] != "#e0c600")
            {
                actual = false;
            }
            if (colorSelection[2] != "#f2aa00")
            {
                actual = false;
            }
            if (colorSelection[3] != "#de2002")
            {
                actual = false;
            }

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
