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

namespace Team121GBNUnitTest
{
    public class GameRecommendatorTests
    {
        private int[] genreCount;
        private Mock<GPDbContext> _mockContext;
        private Mock<DbSet<Game>> _mockGameDbSet;
        private List<Game> _game;

        //In Memory Setup
        public static readonly string _seedFile = @"..\..\..\Data\updatedSeed.sql";
        private InMemoryDbHelper<GPDbContext> _dbHelper = new InMemoryDbHelper<GPDbContext>(_seedFile, DbPersistence.OneDbPerTest);

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGameRepository _gameRepository;
        private readonly IRepository<Game> _genericGameRepo;


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


        //Testing findTopGenres
        [Test]
        public void findTopGenresWithThreeLargeGenresOfIDFiveSevenAndNineShouldReturnTrue()
        {
            //Arrange 
            IGameRecommender _gameRecommender = new GameRecommender(_mockContext.Object);
            genreCount = new int[23] { 0, 0, 0, 0, 5, 0, 8, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            bool result = true;

            //Act
            int[] TopGenres = _gameRecommender.findTopGenres(genreCount);
            if (TopGenres[0] != 9)
            {
                result = false;
            }
            else if (TopGenres[1] != 7)
            {
                result = false;
            }
            else if (TopGenres[2] != 5)
            {
                result = false;
            }

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void findTopGenresWithArrayOfAllZerosShouldReturnTrue()
        {
            //Arrange 
            IGameRecommender _gameRecommender = new GameRecommender(_mockContext.Object);
            int[] genreCount2 = new int[23] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            bool result = true;

            //Act
            int[] TopGenres = _gameRecommender.findTopGenres(genreCount2);
            if (TopGenres[0] != 1)
            {
                result = false;
            }
            else if (TopGenres[1] != 2)
            {
                result = false;
            }
            else if (TopGenres[2] != 3)
            {
                result = false;
            }

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void findTopGenresWithArrayOfAllUniqueNumbersShouldReturnTrue()
        {
            //Arrange 
            IGameRecommender _gameRecommender = new GameRecommender(_mockContext.Object);
            int[] genreCount2 = new int[23] { 1, 6, 7, 8, 7, 10, 4, 17, 59, 23, 13, 18, 26, 24, 27, 39, 35, 55, 0, 30, 40, 70, 25 };
            bool result = true;

            //Act
            int[] TopGenres = _gameRecommender.findTopGenres(genreCount2);
            if (TopGenres[0] != 22)
            {
                result = false;
            }
            else if (TopGenres[1] != 9)
            {
                result = false;
            }
            else if (TopGenres[2] != 18)
            {
                result = false;
            }

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void findTopGenresWithArrayWithFirstPlaceHavingtheSameNumberShouldReturnTrue()
        {
            //Arrange 
            IGameRecommender _gameRecommender = new GameRecommender(_mockContext.Object);
            int[] genreCount2 = new int[23] { 1, 6, 7, 8, 7, 10, 4, 70, 59, 23, 13, 18, 26, 24, 27, 39, 35, 55, 0, 30, 40, 70, 25 };
            bool result = true;

            //Act
            int[] TopGenres = _gameRecommender.findTopGenres(genreCount2);
            if (TopGenres[0] != 8)
            {
                result = false;
            }
            else if (TopGenres[1] != 22)
            {
                result = false;
            }
            else if (TopGenres[2] != 9)
            {
                result = false;
            }

            //Assert
            Assert.IsTrue(result);
        }

        //Testing genreCounter

        [Test] 
        public void genreCounterUsingRealGameInDatabaseShouldReturnTrue()
        {
            //Arrange
            using GPDbContext context = _dbHelper.GetContext();
            IGameRecommender _gameRecommender = new GameRecommender(context);
            GameRepository gameRepository = new GameRepository(context);
            List<Game> games = gameRepository.GetGamesByTitle("Gears Of War");
            Game GameForTesting = games[0];
            int[] genreCount3 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            bool result = true;

            //Act
            _gameRecommender.genreCounter(GameForTesting, genreCount3);

            if (genreCount3[2] != 1)
            {
                result = false;
            }

            Assert.IsTrue(result);
        }

        //Testing calculateNumberOfGames
        [Test]
        public void calculateNumberOfGamesWithNumberOfGamesBeing10AndDivisorBeing2ShouldReturn5()
        {
            //Arrange
            IGameRecommender _gameRecommender = new GameRecommender(_mockContext.Object);

            //Act
            int result = _gameRecommender.calculateNumberOfGames(10, 2);

            //Assert
            Assert.AreEqual(5, result);
        }

        [Test]
        public void calculateNumberOfGamesWithNumberOfGamesBeing10AndDivisorBeing3ShouldReturn3()
        {
            //Arrange
            IGameRecommender _gameRecommender = new GameRecommender(_mockContext.Object);

            //Act
            int result = _gameRecommender.calculateNumberOfGames(10, 3);

            //Assert
            Assert.AreEqual(3, result);
        }

        //Testing getCurratedSection
        [Test]
        public void getCurratedSectionWithPosition5gameTakeCount5ShouldReturn5Games()
        {
            //Arrange
            using GPDbContext context = _dbHelper.GetContext();
            IGameRecommender _gameRecommender = new GameRecommender(context);
            List<Game> emptyList = new List<Game>();

            //Act
            List<Game> games = _gameRecommender.getCurratedSection(5,5, emptyList);

            //Assert
            Assert.AreEqual(games.Count, 5);
        }
/*
        [Test]
        public void getCurratedSectionWithPosition5gameTakeCount3ShouldReturn3Games()
        {
            //Arrange
            using GPDbContext context = _dbHelper.GetContext();
            IGameRecommender _gameRecommender = new GameRecommender(context);
            List<Game> emptyList = new List<Game>();

            //Act
            List<Game> games = _gameRecommender.getCurratedSection(5, 3, emptyList);

            //Assert
            Assert.AreEqual(games.Count, 3);
        }*/

        [Test]
        public void getCurratedSectionWithPosition1gameTakeCount5ShouldReturn0Games()
        {
            //Arrange
            using GPDbContext context = _dbHelper.GetContext();
            IGameRecommender _gameRecommender = new GameRecommender(context);
            List<Game> emptyList = new List<Game>();

            //Act
            List<Game> games = _gameRecommender.getCurratedSection(1, 5, emptyList);

            //Assert
            Assert.AreEqual(games.Count, 0);
        }
    }
}
