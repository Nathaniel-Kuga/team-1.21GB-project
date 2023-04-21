using Team121GBCapstoneProject.Models;
using Team121GBCapstoneProject.Models.DTO;

namespace Team121GBCapstoneProject.Services.Abstract;

public interface IIgdbService
{
    void SetCredentials(string clientId, string token);
    // ! This method might not be needed
    // ! Task<string> ConstructSearchBody(string platform, string genre, int esrbRatingId, string query);
    Task<IEnumerable<IgdbGame>> SearchGames(string query);
    Task<IEnumerable<IgdbGame>> SearchGameWithCachingAsync(int numberOfGames, string platform, string genre, int esrbRating,    string query = "");
    public bool checkGamesFromDatabase(List<Game> gamesToCheck, List<IgdbGame> gamesToReturn, int numberOfGamesToCheck);
    public bool CheckForGame(List<Game> gamesToCheck, string title);
    public void AddGamesToDb(List<Game> GamesFromOurDB, List<IgdbGame> gameFromAPI, List<IgdbGame> gamesToReturn, int numberOfGamesToCheck, string platform, string genre, int esrbRating);
    /// <summary>
    /// Adds new GameGenres to db after having added a new game to db
    /// </summary>
    /// <param name="gameFromApi">IGDB Game object used to add new Game to db</param>
    /// <param name="addedGame">Game object that was just added to the db</param>
    public void AddGameGenreForNewGames(IgdbGame gamesFromApi, Game addedGame);
    /// <summary>
    /// Adds new GamePlatform to db after having added a new game to db
    /// </summary>
    /// <param name="gameFromApi">IGDB Game object used to add new Game to db</param>
    /// <param name="addedGame">Game object that was just added to the db</param>
    public void AddGamePlatformForNewGames(IgdbGame gameFromApi, Game addedGame);
    /// <summary>
    /// Applies the filters provided by the client to the resulting games being returned
    /// </summary>
    /// <param name="games">The list of games made by AddGamesToDb</param>
    /// <param name="platform">Filter provided by client</param>
    /// <param name="genre">Filter provided by client</param>
    /// <param name="esrbRating">Filter provided by client</param>
    /// <returns>Filtered list of igdbgames</returns>
    public List<IgdbGame> ApplyFiltersForNewGames(List<IgdbGame> games, string platform, string genre, int esrbRating);
}
