using Newtonsoft.Json;
using System.Net;
using Microsoft.Net.Http.Headers;
using Team121GBCapstoneProject.Models;
using Team121GBCapstoneProject.Services.Abstract;
using System.Net.Http.Headers;
using Team121GBCapstoneProject.Models.DTO;
using Microsoft.DotNet.MSIdentity.Shared;
using Team121GBCapstoneProject.DAL.Abstract;
using System.Diagnostics;

namespace Team121GBCapstoneProject.Services.Concrete;

public class IgdbService : IIgdbService
{
    // * Inject the IHttpClientFactory through constructor injection
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IGameRepository _gameRepository;
    private readonly IRepository<Game> _genericGameRepo;
    private readonly IRepository<Esrbrating> _esrbRatingRepository;
    private readonly IRepository<GameGenre> _gameGenreRepository;
    private readonly IRepository<Genre> _genreRepository;
    private readonly IRepository<Platform> _platformRepository;
    private readonly IRepository<GamePlatform> _gamePlatformRepository;


    private string _bearerToken;
    private string _clientId;

    public IgdbService(IHttpClientFactory httpClientFactory,
                       IGameRepository gameRepository,
                       IRepository<Game> genericGameRepo,
                       IRepository<Esrbrating> esrbRatingRepository,
                       IRepository<GameGenre> gameGenreRepository,
                       IRepository<Genre> genreRepository,
                       IRepository<GamePlatform> gamePlatformRepository,
                       IRepository<Platform> platformRepository)
    {
        _httpClientFactory = httpClientFactory;
        _gameRepository = gameRepository;
        _genericGameRepo = genericGameRepo;
        _esrbRatingRepository = esrbRatingRepository;
        _gameGenreRepository = gameGenreRepository;
        _genreRepository = genreRepository;
        _gamePlatformRepository = gamePlatformRepository;
        _platformRepository = platformRepository;
    }

    public async Task<string> GetJsonStringFromEndpoint(string token, string uri, string clientId, string rawBody)
    {

        HttpClient httpClient = _httpClientFactory.CreateClient();

        // Add Headers
        httpClient.BaseAddress = new Uri(uri);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpClient.DefaultRequestHeaders.Add("Client-ID", clientId);

        HttpResponseMessage response = await httpClient.PostAsync(uri, new StringContent(rawBody));

        // This is only a minimum version; make sure to cover all your bases here
        if (response.IsSuccessStatusCode)
        {
            // This is blocking; use ReadAsStreamAsync instead
            string responseText = await response.Content.ReadAsStringAsync();
            return responseText;
        }
        else
        {
            throw new HttpRequestException();
        }
    }
    public void SetCredentials(string clientId, string token)
    {
        _clientId = clientId;
        _bearerToken = token;
    }

    public async Task<IEnumerable<IgdbGame>> SearchGames(string query = "")
    {
        // * game Endpoint Search
        string searchBody = $"search \"{query}\";" +
                            "fields name, cover.url, url, summary, first_release_date, rating, age_ratings.rating, age_ratings.category, platforms.name, genres.name;" +
                            " where parent_game = null & age_ratings.category = 1;";
        if (String.IsNullOrEmpty(searchBody)) return Enumerable.Empty<IgdbGame>(); //! If the query was empty don't hit the API cause there is no point in send an empty query.
        string gameSearchUri = "https://api.igdb.com/v4/games/";
        string gameResponse = await GetJsonStringFromEndpoint(_bearerToken, gameSearchUri, _clientId, searchBody);
        IEnumerable<GameJsonDTO> gamesJsonDTO;
        try
        {
            gamesJsonDTO = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<GameJsonDTO>>(gameResponse);
        }
        catch (System.Text.Json.JsonException)
        {
            gamesJsonDTO = null;
        }
        if (gamesJsonDTO != null && gamesJsonDTO.Any())
        {
            try
            {
                return gamesJsonDTO.Select(g => new IgdbGame(g.id,
                                                          g.name,
                                                          g.cover?.url?.ToString(),
                                                          g.url,
                                                          g.summary,
                                                          GameJsonDTO.ConvertFirstReleaseDateFromUnixTimestampToYear(g.first_release_date),
                                                          g.rating,
                                                          GameJsonDTO.ExtractEsrbRatingFromAgeRatingsArray(g.age_ratings),
                                                          g.genres.Select(genre => genre.name).ToList(),
                                                          g.platforms.Select(genre => genre.name).ToList()));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        return Enumerable.Empty<IgdbGame>();
    }

    public bool checkGamesFromDatabase(List<Game> gamesToCheck, List<IgdbGame> gamesToReturn, int numberOfGamesToCheck)
    {
        try
        {
            if (gamesToCheck.Count() > 0)
            {
                if (gamesToCheck.Count() >= numberOfGamesToCheck) // ! 
                {
                    int i = 0;
                    foreach (var game in gamesToCheck)
                    {
                        if (i == numberOfGamesToCheck)
                        {
                            break;
                        }

                        int? yearPublished = GameJsonDTO.ConvertFirstReleaseDateFromUnixTimestampToYear(game.YearPublished);
                        // Look into sending null or 0 instead of 1
                        IgdbGame gameToAdd = new IgdbGame(game.IgdbgameId,
                                                          game.Title,
                                                          game.CoverPicture.ToString(),
                                                          game.Igdburl,
                                                          game.Description,
                                                          game.YearPublished,
                                                          (double)game.AverageRating,
                                                          game.EsrbratingId,
                                                        //   game.IgdbgameId,
                                                          game.GameGenres
                                                              .Select(g => g.Genre.Name)
                                                              .ToList(),
                                                          game.GamePlatforms
                                                              .Select(p => p.Platform.Name)
                                                              .ToList());
                        gamesToReturn.Add(gameToAdd);
                        i++;
                    }
                    return true;
                }
                else // ! 
                {
                    foreach (var game in gamesToCheck)
                    {
                        List<string> genres = game.GameGenres
                                                  .Select(g => g.Genre.Name)
                                                  .ToList();

                        List<string> platforms = game.GamePlatforms
                                                     .Select(g => g.Platform.Name)
                                                     .ToList();
                        IgdbGame gameToAdd = new IgdbGame(game.IgdbgameId,
                                                          game.Title,
                                                          game.CoverPicture.ToString(),
                                                          game.Igdburl,
                                                          game.Description,
                                                          game.YearPublished,
                                                          (double)game.AverageRating,
                                                          game.EsrbratingId,
                                                        //   game.IgdbgameId,
                                                          genres,
                                                          platforms);
                        gamesToReturn.Add(gameToAdd);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
        return false;
    }

    public void AddGamesToDb(List<Game> GamesFromOurDB,
                             List<IgdbGame> gameFromAPI,
                             List<IgdbGame> gamesToReturn,
                             int numberOfGamesToCheck,
                             string platform,
                             string genre,
                             int esrbRating)
    {
        foreach (var game in gameFromAPI)
        {
            try
            {
                if (gamesToReturn.Count() >= numberOfGamesToCheck)
                {
                    break;
                }
                if (CheckForGame(GamesFromOurDB, game.GameTitle) == true)
                {
                    continue;
                }

                Game gameToAdd = new Game();
                gameToAdd.Title = game.GameTitle.ToString();

                if (game.GameCoverArt == null)
                {
                    gameToAdd.CoverPicture = "https://images.igdb.com/igdb/image/upload/t_thumb/nocover.png";
                }
                else
                {
                    gameToAdd.CoverPicture = game.GameCoverArt.ToString();
                }

                gameToAdd.Igdburl = game.GameWebsite.ToString();
                gameToAdd.Description = game.GameDescription.ToString();
                gameToAdd.YearPublished = game.FirstReleaseDate;
                gameToAdd.AverageRating = game.AverageRating;
                gameToAdd.IgdbgameId = (int)game.Id;

                // * This is here to make sure that esrbrating will always be null or an int.
                // * sometimes game from Igdb do not have an esrbrating so this handles that case
                int? esrbRatingId = null;
                if (game.ESRBRatingValue != null)
                {
                    try
                    {
                        esrbRatingId = _esrbRatingRepository.GetAll()
                                                            .FirstOrDefault(esrbRating => esrbRating.IgdbratingValue == game.ESRBRatingValue)!
                                                            .Id;
                    }
                    catch (NullReferenceException e)
                    {
                        Debug.WriteLine(e);
                    }
                }
                gameToAdd.EsrbratingId = esrbRatingId;
                try
                {
                    Game addedGame = _genericGameRepo.AddOrUpdate(gameToAdd);
                    AddGameGenreForNewGames(game, addedGame);
                    AddGamePlatformForNewGames(game, addedGame);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
                gamesToReturn.Add(game);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }

    public async Task<IEnumerable<IgdbGame>> SearchGameWithCachingAsync(int numberOfGames,
                                                                        string platform = "",
                                                                        string genre = "",
                                                                        int esrbRating = 0,
                                                                        string query = "")
    {
        List<IgdbGame> gamesToReturn = new List<IgdbGame>();
        List<Game> gamesFromPersonalDb = _gameRepository.GetGamesByTitle(query);

        bool result = checkGamesFromDatabase(gamesFromPersonalDb, gamesToReturn, numberOfGames);

        if (result == true)
        {
            gamesToReturn = ApplyFiltersForNewGames(gamesToReturn,
                                                platform,
                                                genre,
                                                esrbRating);
            return gamesToReturn.OrderByDescending(x => x.FirstReleaseDate);
        }

        var gamesFromSearch = await SearchGames(query);
        AddGamesToDb(gamesFromPersonalDb,
                     gamesFromSearch.ToList<IgdbGame>(),
                     gamesToReturn,
                     numberOfGames,
                     platform,
                     genre,
                     esrbRating);
        gamesToReturn = ApplyFiltersForNewGames(gamesToReturn,
                                                platform,
                                                genre,
                                                esrbRating);
        return gamesToReturn.OrderByDescending(x => x.FirstReleaseDate);
    }

    public async Task<IEnumerable<IgdbGame>> SpeedSearchAsync(int numberOfGames,
                                                                        string platform = "",
                                                                        string genre = "",
                                                                        int esrbRating = 0,
                                                                        string query = "")
    {
        List<IgdbGame> gamesToReturn = new List<IgdbGame>();

        var gamesFromSearch = await SearchGames(query);
        List<Game> gamesFromPersonalDb = _gameRepository.GetGamesByTitle(query);

        AddGamesToDb(gamesFromPersonalDb, gamesFromSearch.ToList(), gamesToReturn, 10, "", "", -1);



        /*  gamesToReturn = ApplyFiltersForNewGames(gamesToReturn,
                                                  platform,
                                                  genre,
                                                  esrbRating);*/
        return gamesFromSearch.OrderByDescending(x => x.FirstReleaseDate);
    }
    public bool CheckForGame(List<Game> gamesToCheck, string title)
    {
        foreach (var game in gamesToCheck)
        {
            if (game.Title == title)
            {
                return true;
            }
        }
        return false;
    }
    public void AddGameGenreForNewGames(IgdbGame gameFromApi, Game addedGame)
    {
        List<Genre> allGenres = _genreRepository.GetAll().ToList();
        if (gameFromApi.Genres != null)
        {

            List<GameGenre> gameGenresToAddForNewGames = gameFromApi.Genres
                .Select(genre =>
                    allGenres.FirstOrDefault(g =>
                        g.Name == genre)) // ! Returns a list of genres and matches their name and sets all others to null
                .Where(genre => genre != null)
                .Select(genre => new GameGenre
                {
                    GameId = addedGame.Id,
                    Game = addedGame,
                    GenreId = genre.Id,
                    Genre = genre
                })
                .ToList();
            gameGenresToAddForNewGames.ForEach(gg =>
            {
                _gameGenreRepository.AddOrUpdate(gg);
            });
        }
    }
    public void AddGamePlatformForNewGames(IgdbGame gameFromApi, Game addedGame)
    {
        List<Platform> allPlatforms = _platformRepository.GetAll().ToList();
        if (gameFromApi.Platforms != null)
        {
            List<GamePlatform> gamePlatformsToAddForNewGames = gameFromApi.Platforms
                .Select(platform =>
                    allPlatforms.FirstOrDefault(g =>
                        g.Name == platform)) // ! Returns a list of platforms and matches their name and sets all others to null
                .Where(platform => platform != null)
                .Select(platform => new GamePlatform
                {
                    GameId = addedGame.Id,
                    Game = addedGame,
                    PlatformId = platform.Id,
                    Platform = platform
                })
                .ToList();
            gamePlatformsToAddForNewGames.ForEach(gp =>
            {
                _gamePlatformRepository.AddOrUpdate(gp);
            });
        }
    }
    public List<IgdbGame> ApplyFiltersForNewGames(List<IgdbGame> games,
                                        string platform,
                                        string genre,
                                        int esrbRating)
    {
        // * No filters from client just return
        if (string.IsNullOrEmpty(platform) && string.IsNullOrEmpty(genre) && esrbRating == 0) return games;
        // * Otherwise apply filters
        /*
         * Checks in filters are null or empty
         * Then checks if genres or platforms are null, and if not checks to see if they contain provided filters.
         * Then checks if esrbRating is 0 otherwise grab the games that have the rating requested.
         */
        return games.Where(g =>
                          (string.IsNullOrEmpty(genre) || (g.Genres?.Any(x => x == genre) ?? false)) &&
                          (string.IsNullOrEmpty(platform) || (g.Platforms?.Any(x => x == platform) ?? false)) &&
                          (esrbRating == 0 || _esrbRatingRepository.FindById((int)g.ESRBRatingValue).IgdbratingValue == esrbRating))
                    .OrderByDescending(x => x?.FirstReleaseDate)
                    .ToList();
    }
}
