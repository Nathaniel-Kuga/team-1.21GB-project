using Team121GBCapstoneProject.Models;

namespace Team121GBCapstoneProject.Services.Abstract
{
    public interface ISteamChecker
    {
        public bool checkGameTitle(string title, List<PersonGame> games);
        public bool checkGameId(int id, List<PersonGame> games);
    }
}
