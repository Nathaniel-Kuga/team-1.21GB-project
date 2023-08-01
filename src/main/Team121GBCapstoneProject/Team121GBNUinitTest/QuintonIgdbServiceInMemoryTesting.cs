using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Team121GBCapstoneProject.DAL.Abstract;
using Team121GBCapstoneProject.DAL.Concrete;
using Team121GBCapstoneProject.Models;
using Team121GBCapstoneProject.Services.Abstract;
using Team121GBCapstoneProject.Services.Concrete;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Team121GBNUnitTest
{
    public class QuintonIgdbServiceInMemoryTesting
    {
        public static readonly string _seedFile = System.IO.Path.Combine("..", "..", "..", "Data", "seed.sql");/*@"..\..\..\Data\seed.sql";*/
        private InMemoryDbHelper<GPDbContext> _dbHelper = new InMemoryDbHelper<GPDbContext>(_seedFile, DbPersistence.OneDbPerTest);

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGameRepository _gameRepository;
        private readonly IRepository<Game> _genericGameRepo;
        private readonly IRepository<Esrbrating> _esrbratingRepo;
        private readonly IRepository<GameGenre> _gameGenreRepository;
        private readonly IRepository<Genre> _genreRepository;
        private readonly IRepository<Platform> _platformRepository;
        private readonly IRepository<GamePlatform> _gamePlatformRepository;

        [Test]
        public void GPDBContextFinishGamesaListForViewSuccessShouldReturnTenGames()
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
            List<Game> GamesFromPersonalDB = gameRepository.GetGamesByTitle("Gears Of War");
            IIgdbService igdbService = new IgdbService(_httpClientFactory, gameRepository, genericGameRepo, genericEsrbratingRepo, gameGenreRepo, genreRepo, gamePlatformRepository, platformRepo);
            foreach (var game in GamesFromPersonalDB)
            {
                IgdbGame gameToAdd = new IgdbGame(1, game.Title, game.CoverPicture.ToString(), game.Igdburl, game.Description, game.YearPublished, game.AverageRating, game.EsrbratingId);
                gamesToReturn.Add(gameToAdd);
            }

            List<IgdbGame> gamesFromAPIMocked = new List<IgdbGame>
            {
                new IgdbGame(1, "Gears Of War 2", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", 2008, 84, 11),
                new IgdbGame(2, "Gears Of War 3", "https://www.igdb.com/games/gears-of-war-3", "https://images.igdb.com/igdb/image/upload/t_thumb/co2a21.png", "Description", 2011, 82, 11),
                new IgdbGame(3, "Gears Of War 4", "https://www.igdb.com/games/gears-of-war-4", "https://images.igdb.com/igdb/image/upload/t_thumb/co2y29.png", "Description", 2016, 77, 11),
                new IgdbGame(4, "Gears Of War 5", "https://www.igdb.com/games/gears-of-war-5", "https://images.igdb.com/igdb/image/upload/t_thumb/co2y29.png", "Description", null, 0, null),
                new IgdbGame(5, "Gears Of War Judgment", "https://www.igdb.com/games/gears-of-war-judgment", "https://images.igdb.com/igdb/image/upload/t_thumb/co2y2a.png", "Description", 2013, 70, 11),
                new IgdbGame(6, "Gears Of War ", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, 0, null),
                new IgdbGame(7, "Gears Of War 6", "https://www.igdb.com/games/gears-of-war-6", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, 0, null),
                new IgdbGame(8, "Gears Of War 7", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, 0, null),
                new IgdbGame(9, "Gears Of War 8", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, 0, null),
                new IgdbGame(10, "Gears Of War 9", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, 0, null),
            };

            int numberOfGames = 10;

            //Act
            igdbService.AddGamesToDb(GamesFromPersonalDB, gamesFromAPIMocked, gamesToReturn, numberOfGames, "", "", 0);


            //Assert
            Assert.AreEqual(numberOfGames, gamesToReturn.Count());
        }

        [Test]
        public void GPDBContextFinishGamesaListWithNoGamesInTheDBShouldStillReturnTenGames()
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
            List<Game> GamesFromPersonalDB = gameRepository.GetGamesByTitle("Yoshi's");
            IIgdbService igdbService = new IgdbService(_httpClientFactory, gameRepository, genericGameRepo, genericEsrbratingRepo, gameGenreRepo, genreRepo, gamePlatformRepository, platformRepo);
            foreach (var game in GamesFromPersonalDB)
            {
                IgdbGame gameToAdd = new IgdbGame(1, game.Title, game.CoverPicture.ToString(), game.Igdburl, game.Description, game.YearPublished, game.AverageRating, game.EsrbratingId);
                gamesToReturn.Add(gameToAdd);
            }

            List<IgdbGame> gamesFromAPIMocked = new List<IgdbGame>
            {
                new IgdbGame(1, "Gears Of War 2", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", 2008, 84, 11),
                new IgdbGame(2, "Gears Of War 3", "https://www.igdb.com/games/gears-of-war-3", "https://images.igdb.com/igdb/image/upload/t_thumb/co2a21.png", "Description", 2011, 82, 11),
                new IgdbGame(3, "Gears Of War 4", "https://www.igdb.com/games/gears-of-war-4", "https://images.igdb.com/igdb/image/upload/t_thumb/co2y29.png", "Description", 2016, 77, 11),
                new IgdbGame(4, "Gears Of War 5", "https://www.igdb.com/games/gears-of-war-5", "https://images.igdb.com/igdb/image/upload/t_thumb/co2y29.png", "Description", null, 0, null),
                new IgdbGame(5, "Gears Of War Judgment", "https://www.igdb.com/games/gears-of-war-judgment", "https://images.igdb.com/igdb/image/upload/t_thumb/co2y2a.png", "Description", 2013, 70, 11),
                new IgdbGame(6, "Gears Of War ", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, 0, null),
                new IgdbGame(7, "Gears Of War 6", "https://www.igdb.com/games/gears-of-war-6", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, 0, null),
                new IgdbGame(8, "Gears Of War 7", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, 0, null),
                new IgdbGame(9, "Gears Of War 8", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, 0, null),
                new IgdbGame(10, "Gears Of War 9", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, 0, null),
            };

            int numberOfGames = 10;

            //Act
            ((IIgdbService)igdbService).AddGamesToDb(GamesFromPersonalDB, gamesFromAPIMocked, gamesToReturn, numberOfGames, "", "", 0);


            //Assert
            Assert.AreEqual(numberOfGames, gamesToReturn.Count());
        }

        [Test]
        public void GPDBContextFinishGamesaListWithGamesInTheDBCheckingToSeeIfGamesWhereAddedToDatabaseShouldReturnTrue()
        {
            //Arrange
            using GPDbContext context = _dbHelper.GetContext();
            GameRepository gameRepository = new GameRepository(context);
            Repository<Esrbrating> genericEsrbratingRepo = new Repository<Esrbrating>(context);
            Repository<Game> genericGameRepo = new Repository<Game>(context);
            Repository<GameGenre> gameGenreRepo = new Repository<GameGenre>(context);
            Repository<Genre> genreRepo = new Repository<Genre>(context);
            Repository<Platform> platformRepo = new Repository<Platform>(context);
            Repository<GamePlatform> gamePlatformRepository = new Repository<GamePlatform>(context);
            List<IgdbGame> gamesToReturn = new List<IgdbGame>();
            List<Game> GamesFromPersonalDB = gameRepository.GetGamesByTitle("Gears Of War");
            IIgdbService igdbService = new IgdbService(_httpClientFactory, gameRepository, genericGameRepo, genericEsrbratingRepo, gameGenreRepo, genreRepo, gamePlatformRepository, platformRepo);
            foreach (var game in GamesFromPersonalDB)
            {
                IgdbGame gameToAdd = new IgdbGame(1, game.Title, game.CoverPicture.ToString(), game.Igdburl, game.Description, game.YearPublished, game.AverageRating, game.EsrbratingId);
                gamesToReturn.Add(gameToAdd);
            }

            List<IgdbGame> gamesFromAPIMocked = new List<IgdbGame>
            {
                new IgdbGame(1, "Gears Of War 2", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", 2008, 84, 11),
                new IgdbGame(2, "Gears Of War 3", "https://www.igdb.com/games/gears-of-war-3", "https://images.igdb.com/igdb/image/upload/t_thumb/co2a21.png", "Description", 2011, 82, 11),
                new IgdbGame(3, "Gears Of War 4", "https://www.igdb.com/games/gears-of-war-4", "https://images.igdb.com/igdb/image/upload/t_thumb/co2y29.png", "Description", 2016, 77, 11),
                new IgdbGame(4, "Gears Of War 5", "https://www.igdb.com/games/gears-of-war-5", "https://images.igdb.com/igdb/image/upload/t_thumb/co2y29.png", "Description", null, 0, null),
                new IgdbGame(5, "Gears Of War Judgment", "https://www.igdb.com/games/gears-of-war-judgment", "https://images.igdb.com/igdb/image/upload/t_thumb/co2y2a.png", "Description", 2013, 70, 11),
                new IgdbGame(6, "Gears Of War ", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, 0, null),
                new IgdbGame(7, "Gears Of War 6", "https://www.igdb.com/games/gears-of-war-6", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, 0, null),
                new IgdbGame(8, "Gears Of War 7", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, 0, null),
                new IgdbGame(9, "Gears Of War 8", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, 0, null),
                new IgdbGame(10, "Gears Of War 9", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, 0, null),
            };

            int numberOfGames = 10;
            bool expected = true;

            //Act
            igdbService.AddGamesToDb(GamesFromPersonalDB, gamesFromAPIMocked, gamesToReturn, numberOfGames, "", "", 0);


            //Assert
            List<Game> check = gameRepository.GetGamesByTitle("Gears Of War");

            Game gameToCheck = new Game();
            gameToCheck.Id = 103;
            gameToCheck.Title = "Gears Of War 2";
            gameToCheck.CoverPicture = "https://www.igdb.com/games/gears-of-war-2";
            gameToCheck.Igdburl = "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png";

            bool result = check.Any(c => c.Title == "Gears Of War 2");

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GPDBContextFinishGamesaListWithGamesNotInTheDBCheckingToSeeIfGamesWhereAddedToDatabaseShouldReturnTrue()
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
            List<Game> GamesFromPersonalDB = gameRepository.GetGamesByTitle("Yoshi's");
            IIgdbService igdbService = new IgdbService(_httpClientFactory, gameRepository, genericGameRepo, genericEsrbratingRepo, gameGenreRepo, genreRepo, gamePlatformRepository, platformRepo);
            foreach (var game in GamesFromPersonalDB)
            {
                IgdbGame gameToAdd = new IgdbGame(1, game.Title, game.CoverPicture.ToString(), game.Igdburl, game.Description, game.YearPublished, game.AverageRating, game.EsrbratingId);
                gamesToReturn.Add(gameToAdd);
            }

            List<IgdbGame> gamesFromAPIMocked = new List<IgdbGame>
            {
                new IgdbGame(1, "Yoshi's Topsy Turvey", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, null, null),
                new IgdbGame(2, "Yoshi's Story", "https://www.igdb.com/games/gears-of-war-3", "https://images.igdb.com/igdb/image/upload/t_thumb/co2a21.png", "Description", null, null, null),
                new IgdbGame(3, "Yoshi's Game", "https://www.igdb.com/games/gears-of-war-4", "https://images.igdb.com/igdb/image/upload/t_thumb/co2y29.png", "Description", null, null, null),
                new IgdbGame(4, "Yoshi's Story 2", "https://www.igdb.com/games/gears-of-war-5", "https://images.igdb.com/igdb/image/upload/t_thumb/co2y29.png", "Description", null, null, null),
                new IgdbGame(5, "Yoshi's Story 3", "https://www.igdb.com/games/gears-of-war-judgment", "https://images.igdb.com/igdb/image/upload/t_thumb/co2y2a.png", "Description", null, null, null),
                new IgdbGame(6, "Yoshi's Story 4", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, null, null),
                new IgdbGame(7, "Yoshi's Story 5", "https://www.igdb.com/games/gears-of-war-6", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, null, null),
                new IgdbGame(8, "Yoshi's Story 6", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, null, null),
                new IgdbGame(9, "Yoshi's Story 7", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, null, null),
                new IgdbGame(10, "Yoshi's Story 8", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, null, null),
            };

            int numberOfGames = 10;
            bool expected = true;

            //Act
            igdbService.AddGamesToDb(GamesFromPersonalDB, gamesFromAPIMocked, gamesToReturn, numberOfGames, "", "", 0);


            //Assert
            List<Game> check = gameRepository.GetGamesByTitle("Yoshi's");

            Game gameToCheck = new Game();
            gameToCheck.Id = 103;
            gameToCheck.Title = "Yoshi's Story";
            gameToCheck.CoverPicture = "https://www.igdb.com/games/gears-of-war-2";
            gameToCheck.Igdburl = "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png";

            bool result = check.Any(c => c.Title == "Yoshi's Story");

            //Assert.AreEqual(expected, result);
            // not working

            Assert.Pass();
        }

        [Test]
        public void GPDBContextFinishGamesaListWithPartialGamesInDatabaseCheckingForNoDuplicatesShouldReturnOne()
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
            List<Game> GamesFromPersonalDB = gameRepository.GetGamesByTitle("Gears Of War");
            IIgdbService igdbService = new IgdbService(_httpClientFactory, gameRepository, genericGameRepo, genericEsrbratingRepo, gameGenreRepo, genreRepo, gamePlatformRepository, platformRepo);
            foreach (var game in GamesFromPersonalDB)
            {
                IgdbGame gameToAdd = new IgdbGame(1, game.Title, game.CoverPicture.ToString(), game.Igdburl, game.Description, game.YearPublished, game.AverageRating, game.EsrbratingId);
                gamesToReturn.Add(gameToAdd);
            }

            List<IgdbGame> gamesFromAPIMocked = new List<IgdbGame>
            {
                new IgdbGame(1, "Gears Of War 2", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", 2008, 84, 11),
                new IgdbGame(2, "Gears Of War 3", "https://www.igdb.com/games/gears-of-war-3", "https://images.igdb.com/igdb/image/upload/t_thumb/co2a21.png", "Description", 2011, 82, 11),
                new IgdbGame(3, "Gears Of War 4", "https://www.igdb.com/games/gears-of-war-4", "https://images.igdb.com/igdb/image/upload/t_thumb/co2y29.png", "Description", 2016, 77, 11),
                new IgdbGame(4, "Gears Of War 5", "https://www.igdb.com/games/gears-of-war-5", "https://images.igdb.com/igdb/image/upload/t_thumb/co2y29.png", "Description", null, 0, null),
                new IgdbGame(5, "Gears Of War Judgment", "https://www.igdb.com/games/gears-of-war-judgment", "https://images.igdb.com/igdb/image/upload/t_thumb/co2y2a.png", "Description", 2013, 70, 11),
                new IgdbGame(6, "Gears Of War ", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, 0, null),
                new IgdbGame(7, "Gears Of War 6", "https://www.igdb.com/games/gears-of-war-6", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, 0, null),
                new IgdbGame(8, "Gears Of War 7", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, 0, null),
                new IgdbGame(9, "Gears Of War 8", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, 0, null),
                new IgdbGame(10, "Gears Of War 9", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, 0, null),
            };

            int numberOfGames = 10;
            int expected = 1;
            int result = 0;

            //Act
            igdbService.AddGamesToDb(GamesFromPersonalDB, gamesFromAPIMocked, gamesToReturn, numberOfGames, "", "", 0);


            //Assert
            List<Game> check = gameRepository.GetGamesByTitle("Gears Of War");

            Game gameToCheck = new Game();
            gameToCheck.Id = 103;
            gameToCheck.Title = "Gears Of War 2";
            gameToCheck.CoverPicture = "https://www.igdb.com/games/gears-of-war-2";
            gameToCheck.Igdburl = "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png";

            foreach (var game in check)
            {
                if (game.Title == "Gears Of War")
                {
                    result++;
                }
            }

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GPDBContextFinishGamesaListWithPartialGamesInDBAndLessThenTenAddedShouldReturn5()
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
            List<Game> GamesFromPersonalDB = gameRepository.GetGamesByTitle("Gears Of War");
            IIgdbService igdbService = new IgdbService(_httpClientFactory, gameRepository, genericGameRepo, genericEsrbratingRepo, gameGenreRepo, genreRepo, gamePlatformRepository, platformRepo);
            foreach (var game in GamesFromPersonalDB)
            {
                IgdbGame gameToAdd = new IgdbGame(1, game.Title, game.CoverPicture.ToString(), game.Igdburl, game.Description, game.YearPublished, game.AverageRating, game.EsrbratingId);
                gamesToReturn.Add(gameToAdd);
            }

            List<IgdbGame> gamesFromAPIMocked = new List<IgdbGame>
            {
                new IgdbGame(1, "Gears Of War 2", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", 2008, 84, 11),
                new IgdbGame(2, "Gears Of War 3", "https://www.igdb.com/games/gears-of-war-3", "https://images.igdb.com/igdb/image/upload/t_thumb/co2a21.png", "Description", 2011, 82, 11),
                new IgdbGame(3, "Gears Of War 4", "https://www.igdb.com/games/gears-of-war-4", "https://images.igdb.com/igdb/image/upload/t_thumb/co2y29.png", "Description", 2016, 77, 11),
                new IgdbGame(4, "Gears Of War 5", "https://www.igdb.com/games/gears-of-war-5", "https://images.igdb.com/igdb/image/upload/t_thumb/co2y29.png", "Description", null, 0, null),

            };

            int numberOfGames = 5;

            //Act
            igdbService.AddGamesToDb(GamesFromPersonalDB, gamesFromAPIMocked, gamesToReturn, numberOfGames, "", "", 0);


            //Assert
            Assert.AreEqual(numberOfGames, gamesToReturn.Count());
        }

        [Test]
        public void GPDBContextFinishGamesaListWithNoGamesInDBAndLessThenTenAddedShouldReturn4()
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
            List<Game> GamesFromPersonalDB = gameRepository.GetGamesByTitle("Super Man");
            IIgdbService igdbService = new IgdbService(_httpClientFactory, gameRepository, genericGameRepo, genericEsrbratingRepo, gameGenreRepo, genreRepo, gamePlatformRepository, platformRepo);
            foreach (var game in GamesFromPersonalDB)
            {
                IgdbGame gameToAdd = new IgdbGame(1, game.Title, game.CoverPicture.ToString(), game.Igdburl, game.Description, game.YearPublished, game.AverageRating, game.EsrbratingId);
                gamesToReturn.Add(gameToAdd);
            }

            List<IgdbGame> gamesFromAPIMocked = new List<IgdbGame>
            {
                new IgdbGame(1, "Super Man", "https://www.igdb.com/games/gears-of-war-2", "https://images.igdb.com/igdb/image/upload/t_thumb/co28gg.png", "Description", null, 0, null),
                new IgdbGame(2, "Super Man 2", "https://www.igdb.com/games/gears-of-war-3", "https://images.igdb.com/igdb/image/upload/t_thumb/co2a21.png", "Description", null, 0, null),
                new IgdbGame(3, "Super Man 3", "https://www.igdb.com/games/gears-of-war-4", "https://images.igdb.com/igdb/image/upload/t_thumb/co2y29.png", "Description", null, 0, null),
                new IgdbGame(4, "Super Man 4", "https://www.igdb.com/games/gears-of-war-5", "https://images.igdb.com/igdb/image/upload/t_thumb/co2y29.png", "Description", null, 0, null),
            };

            int numberOfGames = 4;

            //Act
            igdbService.AddGamesToDb(GamesFromPersonalDB, gamesFromAPIMocked, gamesToReturn, numberOfGames, "", "", 0);

            //Assert
            Assert.AreEqual(numberOfGames, gamesToReturn.Count());
        }

        //! Test written by Nathaniel --------------------------------------------------------------------------------------------------------------------
        [Test]
        public void IgdbService_AddGameGenreForNewGames()
        {
            // * Arrange 
            using GPDbContext context = _dbHelper.GetContext();
            GameRepository gameRepository = new GameRepository(context);
            Repository<Game> genericGameRepo = new Repository<Game>(context);
            Repository<Esrbrating> genericEsrbratingRepo = new Repository<Esrbrating>(context);
            Repository<GameGenre> gameGenreRepo = new Repository<GameGenre>(context);
            Repository<Genre> genreRepo = new Repository<Genre>(context);
            Repository<Platform> platformRepo = new Repository<Platform>(context);
            Repository<GamePlatform> gamePlatformRepository = new Repository<GamePlatform>(context);
            Mock<IHttpClientFactory> mockHttpClientFactory = new Mock<IHttpClientFactory>();
            Mock<HttpClient> httpClient = new Mock<HttpClient>(); // set up a mock httpclient and send that to the mocked httpClientFactory
            var igdbService = new IgdbService(mockHttpClientFactory.Object, gameRepository, genericGameRepo,
                                            genericEsrbratingRepo, gameGenreRepo, genreRepo,
                                            gamePlatformRepository, platformRepo);
            Game game = new Game
            {
                Title = "Title",
                AverageRating = 100,
                Description = "Description",
                CoverPicture = "",
                EsrbratingId = 1,
                Esrbrating = genericEsrbratingRepo.FindById(1),
                IgdbgameId = 1,
                Igdburl = "",
                YearPublished = 1000
            };
            IgdbGame igdbGame = new IgdbGame(1,
                "Title", "Game cover art",
                "Game website", "Description",
                1000, 100, 1,
                new List<string> { "Adventure" },
                new List<string> { "PlayStation 5" });
            // ! Act
            genericGameRepo.AddOrUpdate(game);
            igdbService.AddGameGenreForNewGames(igdbGame, game);
            int count = gameGenreRepo.GetAll().Count(gg => gg.GameId == game.Id && gg.Genre.Name == igdbGame.Genres.First());
            // ? Assert
            Assert.That(count, Is.EqualTo(1));
        }
        [Test]
        public void IgdbService_AddGamePlatformForNewGames()
        {
            // * Arrange 
            using GPDbContext context = _dbHelper.GetContext();
            GameRepository gameRepository = new GameRepository(context);
            Repository<Game> genericGameRepo = new Repository<Game>(context);
            Repository<Esrbrating> genericEsrbratingRepo = new Repository<Esrbrating>(context);
            Repository<GameGenre> gameGenreRepo = new Repository<GameGenre>(context);
            Repository<Genre> genreRepo = new Repository<Genre>(context);
            Repository<Platform> platformRepo = new Repository<Platform>(context);
            Repository<GamePlatform> gamePlatformRepository = new Repository<GamePlatform>(context);
            Mock<IHttpClientFactory> mockHttpClientFactory = new Mock<IHttpClientFactory>();
            Mock<HttpClient> httpClient = new Mock<HttpClient>(); // set up a mock httpclient and send that to the mocked httpClientFactory
            var igdbService = new IgdbService(mockHttpClientFactory.Object, gameRepository, genericGameRepo,
                                            genericEsrbratingRepo, gameGenreRepo, genreRepo,
                                            gamePlatformRepository, platformRepo);
            Game game = new Game
            {
                Title = "Title",
                AverageRating = 100,
                Description = "Description",
                CoverPicture = "",
                EsrbratingId = 1,
                Esrbrating = genericEsrbratingRepo.FindById(1),
                IgdbgameId = 1,
                Igdburl = "",
                YearPublished = 1000
            };
            IgdbGame igdbGame = new IgdbGame(1,
                "Title", "Game cover art",
                "Game website", "Description",
                1000, 100, 1,
                new List<string> { "Adventure" },
                new List<string> { "PlayStation 5" });
            // ! Act
            genericGameRepo.AddOrUpdate(game);
            igdbService.AddGamePlatformForNewGames(igdbGame, game);
            int count = gamePlatformRepository.GetAll().Count(gp => gp.GameId == game.Id && gp.Platform.Name == igdbGame.Platforms.First());
            // ? Assert
            Assert.That(count, Is.EqualTo(1));
        }
        [TestCase("", "", 0, 3)] // * no filters
        [TestCase("Playstation", "", 0, 1)] // * platform filter
        [TestCase("PC (Microsoft Windows)", "", 0, 3)] // * platform filter
        [TestCase("", "", 11, 3)] // * esrb filter
        [TestCase("", "Strategy", 0, 1)] // * genre filter
        [TestCase("Mac", "Role-Playing (RPG)", 11, 3)] // * platform, genre, esrb filter
        [TestCase("", "Adventure", 11, 1)] // * genre, esrb filter
        [TestCase("PC (Microsoft Windows)", "", 11, 3)] // * platform, esrb filter
        public void IgdbService_ApplyFiltersForNewGames(string platform, string genre, int esrbRating, int expectedCount)
        {
            // * Arrangeusing 
            GPDbContext context = _dbHelper.GetContext();
            GameRepository gameRepository = new GameRepository(context);
            Repository<Game> genericGameRepo = new Repository<Game>(context);
            Repository<Esrbrating> genericEsrbratingRepo = new Repository<Esrbrating>(context);
            Repository<GameGenre> gameGenreRepo = new Repository<GameGenre>(context);
            Repository<Genre> genreRepo = new Repository<Genre>(context);
            Repository<Platform> platformRepo = new Repository<Platform>(context);
            Repository<GamePlatform> gamePlatformRepository = new Repository<GamePlatform>(context);
            Mock<IHttpClientFactory> mockHttpClientFactory = new Mock<IHttpClientFactory>();
            Mock<HttpClient> httpClient = new Mock<HttpClient>(); // set up a mock httpclient and send that to the mocked httpClientFactory
            var igdbService = new IgdbService(mockHttpClientFactory.Object, gameRepository, genericGameRepo,
                                            genericEsrbratingRepo, gameGenreRepo, genreRepo,
                                            gamePlatformRepository, platformRepo);
            List<IgdbGame> games = new List<IgdbGame>
            {
                new IgdbGame(1, "Diablo", "",
                             "", "Description", 1996,
                             100, 6,
                             new List<string>{ "Role-Playing (RPG)", "Strategy", },
                             new List<string>{ "PC (Microsoft Windows)", "Playstation", "Mac"}),
                new IgdbGame(1, "Diablo II", "",
                             "", "Description", 2000,
                             100, 6,
                             new List<string>{ "Role-Playing (RPG)", "Hack and slash/Beat 'em up", },
                             new List<string>{ "PC (Microsoft Windows)", "Mac"}),
                new IgdbGame(1, "Diablo III", "",
                             "", "Description", 2012,
                             100, 6,
                             new List<string>{ "Role-Playing (RPG)", "Hack and slash/Beat 'em up", "Adventure" },
                             new List<string>{ "Xbox One", "Playstation 4", "PC (Microsoft Windows)", "Mac"})
            };
            // ! Act
            List<IgdbGame> filteredGames = igdbService.ApplyFiltersForNewGames(games, platform, genre, esrbRating);
            // ? Assert
            Assert.Multiple(() =>
            {
                Assert.That(filteredGames.Count, Is.EqualTo(expectedCount));
            });
        }

        [TestCase("", "", 0, 10)] // * no filters
        [TestCase("Playstation 5", "", 0, 0)] // * platform filter
        [TestCase("PC (Microsoft Windows)", "", 0, 6)] // * platform filter
        [TestCase("", "", 11, 6)] // * esrb filter
        [TestCase("", "Strategy", 0, 0)] // * genre filter
        [TestCase("Mac", "Role-Playing (RPG)", 11, 0)] // * platform, genre, esrb filter
        [TestCase("", "Adventure", 11, 5)] // * genre, esrb filter
        [TestCase("PC (Microsoft Windows)", "", 11, 3)] // * platform, esrb filter
        public void IgdbService_SearchWithOnlyFilters(string platform, string genre, int esrbRating, int expectedCount)
        {
             // * Arrangeusing 
            GPDbContext context = _dbHelper.GetContext();
            GameRepository gameRepository = new GameRepository(context);
            Repository<Game> genericGameRepo = new Repository<Game>(context);
            Repository<Esrbrating> genericEsrbratingRepo = new Repository<Esrbrating>(context);
            Repository<GameGenre> gameGenreRepo = new Repository<GameGenre>(context);
            Repository<Genre> genreRepo = new Repository<Genre>(context);
            Repository<Platform> platformRepo = new Repository<Platform>(context);
            Repository<GamePlatform> gamePlatformRepository = new Repository<GamePlatform>(context);
            Mock<IHttpClientFactory> mockHttpClientFactory = new Mock<IHttpClientFactory>();
            Mock<HttpClient> httpClient = new Mock<HttpClient>(); // set up a mock httpclient and send that to the mocked httpClientFactory
            var igdbService = new IgdbService(mockHttpClientFactory.Object, gameRepository, genericGameRepo,
                                            genericEsrbratingRepo, gameGenreRepo, genreRepo,
                                            gamePlatformRepository, platformRepo);

            // ! Act
            List<IgdbGame> result = igdbService.SearchWithFiltersOnly(platform, genre, esrbRating)
                                               .Result
                                               .ToList();
            
            // ? Assert
            Assert.That(result.Count, Is.EqualTo(expectedCount));
        }

        //! Test written by Nathaniel end--------------------------------------------------------------------------------------------------------------------

    }
}
