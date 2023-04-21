using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Team121GBCapstoneProject.DAL.Abstract;
using Team121GBCapstoneProject.Models;

namespace Team121GBCapstoneProject.DAL.Concrete
{
    public class GameRecommender : IGameRecommender
    {
        private int[] genreCount = new int[23] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 };

        private DbSet<Game> _game;
        public GameRecommender(GPDbContext context)
        {
            _game = context.Games;
        }
        public PersonGame AddOrUpdate(PersonGame entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(PersonGame entity)
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

        public PersonGame FindById(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PersonGame> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<PersonGame> GetAll(params Expression<Func<PersonGame, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        //counting genres
        public void genreCounter(Game gameToCheckGenres, int[] genreArrForCounting) //Tested
        {

            if (gameToCheckGenres.GameGenres.Count != 0)
            {
                int GenreID = (int)gameToCheckGenres.GameGenres.First().GenreId;
                genreArrForCounting[GenreID - 1]++;
            }

        }

        public int[] findTopGenres(int[] genreArr) //Tested
        {
            int[] topGenresToReturn = new int[3];

            for (int i = 0; i < 3; i++)
            {
                int largest = genreArr.Max();
                for (int j = 0; j < genreArr.Length; j++)
                {
                    if (genreArr[j] == largest)
                    {
                        int test = j;
                        topGenresToReturn[i] = (test + 1);
                        genreArr[j] = -1;
                        break;
                    }
                }
            }

            return topGenresToReturn;
        }

        public int calculateNumberOfGames(int numberOfGames, int divisor) //Tested
        {
            return numberOfGames / divisor;
        }

        public List<Game> getCurratedSection(int position, int gameTakeCount, List<Game> prevousList)
        {
            List<Game> listToShuffle = _game.Where(g => g.GameGenres.Any(genre => genre.GenreId == position)).ToList();
            
            List<Game> listToReturn = new List<Game>();

            //fix duplication bug
            if (listToShuffle.Count > 0)
            {
                var random = new Random();
                for (int i = 0; i < gameTakeCount; i++)
                {
                    
                    int randomGameIndex = random.Next(0, listToShuffle.Count());
                    Game gameForCheck = listToShuffle.ElementAt(randomGameIndex);

                    if (listToReturn.Contains(gameForCheck) && prevousList.Contains(gameForCheck))
                    {
                        i--;
                        continue;
                    }
                    else { 
                        listToReturn.Add(listToShuffle.ElementAt(randomGameIndex));
                    }
                }
            }
            return listToReturn.Distinct().ToList();


            //shuffle list
            /* List<Game> listToReturn = _game.Where(g => g.GameGenres.Any(genre => genre.GenreId == position)).Take(gameTakeCount).ToList();
             return listToReturn;*/
        }

        //function to currate list of games
        public List<Game> currateGames(int numberOfGames, List<PersonGame> ownedGames)
        {
            List<Game> curratedGames = new List<Game>();
            List<Game> listForChecking = new List<Game>();
            
            int topGenreGameCount = calculateNumberOfGames(numberOfGames, 2);
            int SecondGenreGameCount = calculateNumberOfGames(numberOfGames, 3);
            int thirdGenreGameCount = numberOfGames - (topGenreGameCount + SecondGenreGameCount);

            //find top x games
            int[] TopGenres = findTopGenres(genreCount);
            int first = TopGenres[0];
            int second = TopGenres[1];
            int third = TopGenres[2];

            curratedGames = getCurratedSection(first, topGenreGameCount, listForChecking);
            listForChecking = curratedGames;
            List<Game> secondPlaceGames = getCurratedSection(second, SecondGenreGameCount, listForChecking);
            listForChecking.AddRange(secondPlaceGames);
            List<Game> thirdPlaceGames = getCurratedSection(third, thirdGenreGameCount, listForChecking);


            curratedGames.AddRange(secondPlaceGames);
            curratedGames.AddRange(thirdPlaceGames);

            return curratedGames.Distinct().ToList();
        }

        //Setting up Array
        public void SetUpGenreCountArray(int numberOfGenres)
        {
            genreCount = new int[numberOfGenres];

            for (int i = 0; i < numberOfGenres; i++)
            {
                genreCount[i] = 0;
            }
        }

        public List<Game> recommendGames(List<PersonGame> games, int numberOfRecommendations)
        {
            List<Game> gamesToReturn= new List<Game>();
            //SetUpGenreCountArray(genreCount);

            if (games.Count > 0)
            {
                //checking for each game in the library
                foreach (var game in games)
                {
                    //Grab the game itself for checking
                    Game gameToCheck = game.Game;

                    //iterate through games genres and count them up
                    genreCounter(gameToCheck, genreCount);
                }

                //generate currated list
                gamesToReturn = currateGames(numberOfRecommendations, games);
            }


            return gamesToReturn.Take(numberOfRecommendations).ToList();
        }
    }
}
