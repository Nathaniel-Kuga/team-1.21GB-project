namespace Team121GBCapstoneProject.Models.DTO
{
    public class GameJsonDTO
    {
        public int id { get; set; }
        public Cover cover { get; set; }
        public string name { get; set; }
        public string url { get; set; }

    }

    public class Cover
    {
        public int id { get; set; }
        public string url { get; set; }
    }
}
