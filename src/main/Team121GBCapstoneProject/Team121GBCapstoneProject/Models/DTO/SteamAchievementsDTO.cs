namespace Team121GBCapstoneProject.Models.DTO
{
    public class SteamAchievementsDTO
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Achievement
        {
            public string apiname { get; set; }
            public int achieved { get; set; }
            public int unlocktime { get; set; }
        }

        public class Playerstats
        {
            public string steamID { get; set; }
            public string gameName { get; set; }
            public List<Achievement> achievements { get; set; }
            public bool success { get; set; }
        }

        public class Root
        {
            public Playerstats playerstats { get; set; }
        }


    }
}
