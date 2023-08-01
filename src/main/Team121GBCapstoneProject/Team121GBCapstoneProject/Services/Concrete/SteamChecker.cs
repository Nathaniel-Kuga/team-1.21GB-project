using Team121GBCapstoneProject.Models;
using Team121GBCapstoneProject.Services.Abstract;

namespace Team121GBCapstoneProject.Services.Concrete
{
    public class SteamChecker : ISteamChecker
    {
        public bool checkGameId(int id, List<PersonGame> games)
        {
            foreach (var personGameCheck in games)
            {
                if (personGameCheck.GameId == id)
                {
                    return true;
                }
            }
            return false;
        }

        public bool checkGameTitle(string title, List<PersonGame> games)
        {
            foreach (var gametoCheckDup in games)
            {
                if (gametoCheckDup.Game.Title == title)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
