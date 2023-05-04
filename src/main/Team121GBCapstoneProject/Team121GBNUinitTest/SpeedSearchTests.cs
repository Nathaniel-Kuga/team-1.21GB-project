using AngleSharp;
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

namespace Team121GBNUnitTest
{
    public class SpeedSearchTests
    {
        public static readonly string _seedFile = @"..\..\..\Data\seed.sql";
        private InMemoryDbHelper<GPDbContext> _dbHelper = new InMemoryDbHelper<GPDbContext>(_seedFile, DbPersistence.OneDbPerTest);

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGameRepository _gameRepository;
        private readonly IRepository<Game> _genericGameRepo;
        private readonly IRepository<Esrbrating> _esrbratingRepo;
        private readonly IRepository<GameGenre> _gameGenreRepository;
        private readonly IRepository<Genre> _genreRepository;
        private readonly IRepository<Platform> _platformRepository;
        private readonly IRepository<GamePlatform> _gamePlatformRepository;
        private IIgdbService _igdbService;


        //Testing TitleParse
        [Test]
        public void TitleParseGivenAEmptyInputShouldReturnEmptyList()
        {
            //Arrange
            List<string> listToCheck = new List<string>();
            using GPDbContext context = _dbHelper.GetContext();

            _igdbService = new IgdbService(_httpClientFactory, _gameRepository, _genericGameRepo, _esrbratingRepo, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository);

            SpeedSearch speedSearch = new SpeedSearch(context, _igdbService);

            //Act
            listToCheck = speedSearch.TitleParse("");

            //Assert
            Assert.AreEqual(0,listToCheck.Count());
        }

        [Test]
        public void TitleParseGivenAValidInputShouldReturnListOfCount2()
        {
            //Arrange
            List<string> listToCheck = new List<string>();
            using GPDbContext context = _dbHelper.GetContext();
            _igdbService = new IgdbService(_httpClientFactory, _gameRepository, _genericGameRepo, _esrbratingRepo, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository);
            SpeedSearch speedSearch = new SpeedSearch(context, _igdbService);

            //Act
            listToCheck = speedSearch.TitleParse("*Super Mario Brothers *Super Mario 64");

            //Assert
            Assert.AreEqual(2, listToCheck.Count());
        }

        [Test]
        public void TitleParseGivenAValidInputShouldReturnListOfCount1()
        {
            //Arrange
            using GPDbContext context = _dbHelper.GetContext();
            _igdbService = new IgdbService(_httpClientFactory, _gameRepository, _genericGameRepo, _esrbratingRepo, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository);

            SpeedSearch speedSearch = new SpeedSearch(context, _igdbService);
            List<string> listToCheck = new List<string>();

            //Act
            listToCheck = speedSearch.TitleParse("*Super Mario Brothers");

            //Assert
            Assert.AreEqual(1, listToCheck.Count());
        }

        [Test]
        public void TitleParseGivenAValidInputWithSpaceBeforeShouldReturnListOfCount1()
        {
            //Arrange
            using GPDbContext context = _dbHelper.GetContext();
            _igdbService = new IgdbService(_httpClientFactory, _gameRepository, _genericGameRepo, _esrbratingRepo, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository);

            SpeedSearch speedSearch = new SpeedSearch(context, _igdbService);
            List<string> listToCheck = new List<string>();

            //Act
            listToCheck = speedSearch.TitleParse("* Super Mario Brothers");

            //Assert
            Assert.AreEqual(1, listToCheck.Count());
        }

        [Test]
        public void TitleParseGivenAValidInputWithSpacesBeforeAndAfterShouldReturnTrue()
        {
            //Arrange
            using GPDbContext context = _dbHelper.GetContext();
            _igdbService = new IgdbService(_httpClientFactory, _gameRepository, _genericGameRepo, _esrbratingRepo, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository);

            SpeedSearch speedSearch = new SpeedSearch(context, _igdbService);
            List<string> listToCheck = new List<string>();

            //Act
            listToCheck = speedSearch.TitleParse("* Super Mario Brothers * Super Mario 64 ");

            //Assert
            bool expected = true;
            bool result = true;

            foreach (string s in listToCheck)
            {
                if (s.StartsWith(' ') || s.EndsWith(' '))
                {
                    result = false;
                    break;
                }
            }

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TitleParseGivenAValidInputWithTwoStarsBeforeTitleShouldReturnListOfCount1()
        {
            //Arrange
            using GPDbContext context = _dbHelper.GetContext();
            _igdbService = new IgdbService(_httpClientFactory, _gameRepository, _genericGameRepo, _esrbratingRepo, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository);

            SpeedSearch speedSearch = new SpeedSearch(context, _igdbService);
            List<string> listToCheck = new List<string>();

            //Act
            listToCheck = speedSearch.TitleParse("**Super Mario Brothers");

            //Assert
            Assert.AreEqual(1, listToCheck.Count());
        }

        [Test]
        public void TitleParseGivenAValidInputWithTwoStarsAndSpaceBetweenBeforeTitleShouldReturnListOfCount1()
        {
            //Arrange
            using GPDbContext context = _dbHelper.GetContext();
            _igdbService = new IgdbService(_httpClientFactory, _gameRepository, _genericGameRepo, _esrbratingRepo, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository);

            SpeedSearch speedSearch = new SpeedSearch(context, _igdbService);
            List<string> listToCheck = new List<string>();

            //Act
            listToCheck = speedSearch.TitleParse("**Super Mario Brothers");

            //Assert
            Assert.AreEqual(1, listToCheck.Count());
        }

        //Testing GetFirstSearchResult
/*        [Test]
        public async Task GetFirstSearchResultWithSuperMarioBrosAsQueryShouldNotBeNullAsync()
        {
            //Arrange
            using GPDbContext context = _dbHelper.GetContext();
            GameRepository gameRepository = new GameRepository(context);
            Repository<Game> genericGameRepo = new Repository<Game>(context);
            Repository<Esrbrating> genericEsrbratingRepo = new Repository<Esrbrating>(context);
            Repository<GameGenre> gameGenreRepo = new Repository<GameGenre>(context);
            Repository<Genre> genreRepo = new Repository<Genre>(context);
            Repository<Platform> platformRepo = new Repository<Platform>(context);
            Repository<GamePlatform> gamePlatformRepository = new Repository<GamePlatform>(context);
            List<IgdbGame> gamesToReturn = new List<IgdbGame>();
            _igdbService = new IgdbService(_httpClientFactory, gameRepository, genericGameRepo, genericEsrbratingRepo, gameGenreRepo, genreRepo, gamePlatformRepository, platformRepo);

            SpeedSearch speedSearch = new SpeedSearch(context, _igdbService);

            //Act
            IgdbGame gameToCheck = await speedSearch.GetFirstSearchResultAsync("Super Mario 64");

            //Assert
            bool expected = true;
            bool result = true;

            if (gameToCheck == null)
            {
                result = false;
            }
            Assert.AreEqual(expected, result);
        }*/

        [Test]
        public async Task GetFirstSearchResultWithInvalidGameAsQueryShouldBeNullAsync()
        {
            //Arrange
            using GPDbContext context = _dbHelper.GetContext();
            GameRepository gameRepository = new GameRepository(context);
            Repository<Game> genericGameRepo = new Repository<Game>(context);
            Repository<Esrbrating> genericEsrbratingRepo = new Repository<Esrbrating>(context);
            Repository<GameGenre> gameGenreRepo = new Repository<GameGenre>(context);
            Repository<Genre> genreRepo = new Repository<Genre>(context);
            Repository<Platform> platformRepo = new Repository<Platform>(context);
            Repository<GamePlatform> gamePlatformRepository = new Repository<GamePlatform>(context);
            List<IgdbGame> gamesToReturn = new List<IgdbGame>();
            _igdbService = new IgdbService(_httpClientFactory, _gameRepository, _genericGameRepo, _esrbratingRepo, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository);

            SpeedSearch speedSearch = new SpeedSearch(context, _igdbService);

            //Act
            IgdbGame gameToCheck = await speedSearch.GetFirstSearchResultAsync("dkjvnskjdcnoiwncdjksnckeslnjvrkjsenckdncs123456789");

            //Assert
            bool expected = false;
            bool result = true;

            if (gameToCheck == null)
            {
                result = false;
            }
            Assert.AreEqual(expected, result);
        }

        //Testing SpeedSearch
 /*       [Test]
        public async Task SpeedSearchWithTwoMarioGamesListShouldHaveACountOf2Async()
        {
            //Arrange
            using GPDbContext context = _dbHelper.GetContext();
            GameRepository gameRepository = new GameRepository(context);
            Repository<Game> genericGameRepo = new Repository<Game>(context);
            Repository<Esrbrating> genericEsrbratingRepo = new Repository<Esrbrating>(context);
            Repository<GameGenre> gameGenreRepo = new Repository<GameGenre>(context);
            Repository<Genre> genreRepo = new Repository<Genre>(context);
            Repository<Platform> platformRepo = new Repository<Platform>(context);
            Repository<GamePlatform> gamePlatformRepository = new Repository<GamePlatform>(context);
            List<IgdbGame> gamesToReturn = new List<IgdbGame>();
            _igdbService = new IgdbService(_httpClientFactory, gameRepository, genericGameRepo, genericEsrbratingRepo, gameGenreRepo, genreRepo, gamePlatformRepository, platformRepo);

            SpeedSearch speedSearch = new SpeedSearch(context, _igdbService);

            //Act
            var listToCheck = await speedSearch.SpeedSearchingAsync("*Super Mario Bros *Super Mario 64");


            //Assert
            Assert.AreEqual(2, listToCheck.Count());
        }*/
    }
}
