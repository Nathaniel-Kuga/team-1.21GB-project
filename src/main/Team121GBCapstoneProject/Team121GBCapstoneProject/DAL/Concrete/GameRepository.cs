using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Linq.Expressions;
using Team121GBCapstoneProject.DAL.Abstract;
using Team121GBCapstoneProject.Models;
using FuzzySharp;
using NuGet.DependencyResolver;

namespace Team121GBCapstoneProject.DAL.Concrete
{
    public class GameRepository : IGameRepository
    {
        private DbSet<Game> _game;
        public GameRepository(GPDbContext context)
        {
            _game = context.Games;
        }

        public Game AddOrUpdate(Game entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Game entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(int id)
        {
            throw new NotImplementedException();
        }

        public Game FindById(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Game> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Game> GetAll(params Expression<Func<Game, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public List<Game> GetGamesByTitle(string title)
        {
            if (title == "")
            {
                List<Game> result = new List<Game>();
                return result;
            }
            var gamesToReturn = _game.Where(g => g.Title.Contains(title)).ToList();
            // Check Fuzzy equality of strings
            if (gamesToReturn.Count == 0)
            {
                List<Game> allGames = _game.ToList();
                gamesToReturn = allGames.Where(game =>
                {
                    int score = Fuzz.Ratio(game.Title, title);
                    return score > 70;
                }).ToList();

            }
            return gamesToReturn;
        }


        public List<Game> GetTrendingGames(int numberOfGames)
        {
            var gamesReturn = _game.OrderByDescending(g => g.AverageRating).Take(numberOfGames).ToList();
            return gamesReturn;
        }

    }
}
