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
using Team121GBCapstoneProject.Services.Abstract;
using Team121GBCapstoneProject.Services.Concrete;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Team121GBNUnitTest
{
    public class QuintonIgdbServiceTests
    {
        private Mock<GPDbContext> _mockContext;
        private Mock<DbSet<Game>> _mockGameDbSet;
        private List<Game> _game;
        private List<Game> _fullGameList;

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGameRepository _gameRepository;
        private readonly IRepository<Game> _genericGameRepo;
        private readonly IRepository<Esrbrating> _esrbRatingRepository;
        private readonly IRepository<GameGenre> _gameGenreRepository;
        private readonly IRepository<Genre> _genreRepository;
        private readonly IRepository<Platform> _platformRepository;
        private readonly IRepository<GamePlatform> _gamePlatformRepository;


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

        //testing if there are some games in the repo but not all match criteria
        [Test]
        public void checkGamesFromDatabaseWithGamesWithOutTenGamesInDataBaseShouldReturnFalse()
        {
            //Arrange
            IIgdbService _igdbService = new IgdbService(_httpClientFactory, _gameRepository, _genericGameRepo, _esrbRatingRepository, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository);
            IGameRepository gameRepository = new GameRepository(_mockContext.Object);
            List<IgdbGame> gamesToReturn = new List<IgdbGame>();
            List<Game> GamesFromPersonalDB = gameRepository.GetGamesByTitle("Dark");
            int numberOfGames = 10;
            bool expected = false;
            bool result;

            //Act
            result = _igdbService.checkGamesFromDatabase(GamesFromPersonalDB, gamesToReturn, numberOfGames);

            //Assert
            Assert.AreEqual(expected, result);

        }

        [Test]
        public void checkGamesFromDatabaseWithNoGamesThatMatchInDatabaseShouldReturnFalse()
        {
            //Arrange
            IIgdbService _igdbService = new IgdbService(_httpClientFactory, _gameRepository, _genericGameRepo, _esrbRatingRepository, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository);;
            IGameRepository gameRepository = new GameRepository(_mockContext.Object);
            List<IgdbGame> gamesToReturn = new List<IgdbGame>();
            List<Game> GamesFromPersonalDB = gameRepository.GetGamesByTitle("Donkey Kong");
            int numberOfGames = 10;
            bool expected = false;
            bool result;

            //Act
            result = _igdbService.checkGamesFromDatabase(GamesFromPersonalDB, gamesToReturn, numberOfGames);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void checkGamesFromDatabaseWithTenGamesThatMatchShouldReturnTrue()
        {
            //Arrange
            //setting up a new mock list
            _fullGameList = new List<Game>
            {
                new Game{ Id = 1, Title = "Dark Souls", Description = "This is Gears of War", YearPublished = 2001, AverageRating = 10, EsrbratingId = 1, CoverPicture = "TesterFile.png"},
                new Game{ Id = 2, Title = "Dark Souls 2", Description = "Dwarfs in a cave", YearPublished = 2019, AverageRating = 9.8, EsrbratingId = 1, CoverPicture = "TesterFile2.png"},
                new Game{ Id = 3, Title = "Dark Souls 3", Description = "Mine Game", YearPublished = 2007, AverageRating = 9.7, EsrbratingId = 1, CoverPicture = "TesterFile3.png"},
                new Game{ Id = 4, Title = "Dark Souls 4", Description = "Too Many Spoliers", YearPublished = 2010, AverageRating = 9.6, EsrbratingId = 1, CoverPicture = "TesterFile4.png"},
                new Game{ Id = 5, Title = "Dark Souls 5", Description = "RPG with a lot of paths", YearPublished = 2020, AverageRating = 8.5, EsrbratingId = 1, CoverPicture = "TesterFile5.png"},
                new Game{ Id = 6, Title = "Dark Souls 6", Description = "Hard Game with hard controls", YearPublished = 2017, AverageRating = 5.7, EsrbratingId = 1, CoverPicture = "TesterFile6.png" },
                new Game{ Id = 7, Title = "Dark Souls 7", Description = "Hard but fun game", YearPublished = 2005, EsrbratingId= 1, AverageRating = 8.5, CoverPicture = "TesterFile7.png"},
                new Game{ Id = 8, Title = "Dark Souls 8", Description = "Worst Game in the series", YearPublished = 2007, EsrbratingId = 1, AverageRating = 4.6, CoverPicture = "TesterFile8.png"},
                new Game{ Id = 9, Title = "Dark Souls 9", Description = "Best Dark Souls Game", YearPublished = 2010, EsrbratingId = 1, AverageRating = 7.9, CoverPicture = "TesterFile9.png"},
                new Game{ Id = 10, Title = "Dark Souls 10", Description = "Bloodborne with eldritch horrors", YearPublished = 2019, EsrbratingId = 1, AverageRating = 9.5, CoverPicture = "TesterFile10.png"},
            };
            _mockContext = new Mock<GPDbContext>();
            _mockGameDbSet = MockHelpers.GetMockDbSet(_fullGameList.AsQueryable());
            _mockContext.Setup(ctx => ctx.Games).Returns(_mockGameDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<Game>()).Returns(_mockGameDbSet.Object);
            
            IIgdbService _igdbService = new IgdbService(_httpClientFactory, _gameRepository, _genericGameRepo, _esrbRatingRepository, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository);;
            IGameRepository gameRepository = new GameRepository(_mockContext.Object);
            List<IgdbGame> gamesToReturn = new List<IgdbGame>();
            List<Game> GamesFromPersonalDB = gameRepository.GetGamesByTitle("Dark");
            int numberOfGames = 10;
            bool expected = true;
            bool result;

            //Act
            result = _igdbService.checkGamesFromDatabase(GamesFromPersonalDB, gamesToReturn, numberOfGames);

            //Assert
            Assert.AreEqual(expected, result);

        }

        [Test]
        public void checkGamesFromDatabaseWithNonSenseQueryShouldReturnFalse()
        {
            //Arrange
            IIgdbService _igdbService = new IgdbService(_httpClientFactory, _gameRepository, _genericGameRepo, _esrbRatingRepository, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository);;
            IGameRepository gameRepository = new GameRepository(_mockContext.Object);
            List<IgdbGame> gamesToReturn = new List<IgdbGame>();
            List<Game> GamesFromPersonalDB = gameRepository.GetGamesByTitle("dkvjnskdvcnbseiukjrnvksnfd");
            int numberOfGames = 10;
            bool expected = false;
            bool result;

            //Act
            result = _igdbService.checkGamesFromDatabase(GamesFromPersonalDB, gamesToReturn, numberOfGames);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void checkGamesFromDatabaseWithEmptyQueryShouldReturnFalse()
        {
            //Arrange
            IIgdbService _igdbService = new IgdbService(_httpClientFactory, _gameRepository, _genericGameRepo, _esrbRatingRepository, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository);;
            IGameRepository gameRepository = new GameRepository(_mockContext.Object);
            List<IgdbGame> gamesToReturn = new List<IgdbGame>();
            List<Game> GamesFromPersonalDB = gameRepository.GetGamesByTitle("");
            int numberOfGames = 0;
            bool expected = false;
            bool result;

            //Act
            result = _igdbService.checkGamesFromDatabase(GamesFromPersonalDB, gamesToReturn, numberOfGames);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void checkGamesFromDatabaseWithWhiteSpaceQueryShouldReturnFalse()
        {
            //Arrange
            IIgdbService _igdbService = new IgdbService(_httpClientFactory, _gameRepository, _genericGameRepo, _esrbRatingRepository, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository);;
            IGameRepository gameRepository = new GameRepository(_mockContext.Object);
            List<IgdbGame> gamesToReturn = new List<IgdbGame>();
            List<Game> GamesFromPersonalDB = gameRepository.GetGamesByTitle(" ");
            int numberOfGames = 10;
            bool expected = false;
            bool result;

            //Act
            result = _igdbService.checkGamesFromDatabase(GamesFromPersonalDB, gamesToReturn, numberOfGames);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CheckForGameWithMatchInTheDatabaseShouldReturnTrue()
        {
            //Arrange
            IRepository<Game> genericGameRepo = new Repository<Game>(_mockContext.Object);
            IIgdbService _igdbService = new IgdbService(_httpClientFactory, _gameRepository, genericGameRepo, _esrbRatingRepository, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository); ;
            IGameRepository gameRepository = new GameRepository(_mockContext.Object);
            List<IgdbGame> gamesToReturn = new List<IgdbGame>();
            List<Game> GamesFromPersonalDB = gameRepository.GetGamesByTitle("Dark Souls");
            bool result;

            //Act
            result = _igdbService.CheckForGame(GamesFromPersonalDB, "Dark Souls 2", 1);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CheckForGameWithOutMatchInTheDatabaseShouldReturnTrue()
        {
            //Arrange
            IRepository<Game> genericGameRepo = new Repository<Game>(_mockContext.Object);
            IIgdbService _igdbService = new IgdbService(_httpClientFactory, _gameRepository, genericGameRepo, _esrbRatingRepository, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository); ;
            IGameRepository gameRepository = new GameRepository(_mockContext.Object);
            List<IgdbGame> gamesToReturn = new List<IgdbGame>();
            List<Game> GamesFromPersonalDB = gameRepository.GetGamesByTitle("Dark Souls");
            bool result;

            //Act
            result = _igdbService.CheckForGame(GamesFromPersonalDB, "Dark Souls 5", 1);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void CheckForGameWithEmptySearchInTheDatabaseShouldReturnFalse()
        {
            //Arrange
            IRepository<Game> genericGameRepo = new Repository<Game>(_mockContext.Object);
            IIgdbService _igdbService = new IgdbService(_httpClientFactory, _gameRepository, genericGameRepo, _esrbRatingRepository, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository);;
            IGameRepository gameRepository = new GameRepository(_mockContext.Object);
            List<IgdbGame> gamesToReturn = new List<IgdbGame>();
            List<Game> GamesFromPersonalDB = gameRepository.GetGamesByTitle("");
            bool result;

            //Act
            result = _igdbService.CheckForGame(GamesFromPersonalDB, "Dark Souls 5", 1);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void CheckForGameWithWhiteSpaceSearchInTheDatabaseShouldReturnFalse()
        {
            //Arrange
            IRepository<Game> genericGameRepo = new Repository<Game>(_mockContext.Object);
            IIgdbService _igdbService = new IgdbService(_httpClientFactory, _gameRepository, genericGameRepo, _esrbRatingRepository, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository); ;
            IGameRepository gameRepository = new GameRepository(_mockContext.Object);
            List<IgdbGame> gamesToReturn = new List<IgdbGame>();
            List<Game> GamesFromPersonalDB = gameRepository.GetGamesByTitle(" ");
            bool result;

            //Act
            result = _igdbService.CheckForGame(GamesFromPersonalDB, "Dark Souls 5", 1);

            //Assert
            Assert.IsFalse(result);
        }

    }
}
