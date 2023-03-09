using Newtonsoft.Json;

namespace Team121GBCapstoneProject.Models
{
    public class IgdbGame
    {
        public int Id { get; set; }
        public string GameTitle { get; set; }
        public string GameCoverArt { get; set; } 
        public string GameWebsite { get; set; }

        public IgdbGame(int id, string gameTitle, string gameCoverArt, string gameWebsite)
        {
            Id = id;
            GameTitle = gameTitle;
            GameCoverArt = gameCoverArt;
            GameWebsite = gameWebsite;
        }
    }
}
