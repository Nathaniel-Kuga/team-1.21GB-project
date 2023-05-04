namespace Team121GBCapstoneProject.Models.DTO
{
    public class SteamAchievementsExtraDTO
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Achievement
        {
            public string name { get; set; }
            public int defaultvalue { get; set; }
            public string displayName { get; set; }
            public int hidden { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
            public string icongray { get; set; }
        }

        public class AvailableGameStats
        {
            public List<Achievement> achievements { get; set; }
            public List<Stat> stats { get; set; }
        }

        public class Game
        {
            public string gameName { get; set; }
            public string gameVersion { get; set; }
            public AvailableGameStats availableGameStats { get; set; }
        }

        public class Root
        {
            public Game game { get; set; }
        }

        public class Stat
        {
            public string name { get; set; }
            public int defaultvalue { get; set; }
            public string displayName { get; set; }
        }


    }
}
