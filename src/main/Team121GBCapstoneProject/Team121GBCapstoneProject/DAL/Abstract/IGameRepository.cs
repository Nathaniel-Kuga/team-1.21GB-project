using Team121GBCapstoneProject.Models;

namespace Team121GBCapstoneProject.DAL.Abstract
{
    public interface IGameRepository : IRepository<Game>
    {
        public List<Game> GetTrendingGames(int numberOfGames);
        public List<Game> GetGamesByTitle(string title);


    }
}
