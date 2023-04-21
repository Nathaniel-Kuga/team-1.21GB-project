using Team121GBCapstoneProject.Models;

namespace Team121GBCapstoneProject.DAL.Abstract
{
    public interface IGameRecommender : IRepository<PersonGame>
    {
        public List<Game> recommendGames(List<PersonGame> games, int numberOfRecommendations);
        public void genreCounter(Game gameToCheckGenres, int[] genreArrForCounting);
        public int[] findTopGenres(int[] genreArr);
        public List<Game> currateGames(int numberOfGames, List<PersonGame> ownedGames);
        public void SetUpGenreCountArray(int numberOfGenres);
        public int calculateNumberOfGames(int numberOfGames, int divisor);
        public List<Game> getCurratedSection(int position, int gameTakeCount, List<Game> prevousList);

    }
}
