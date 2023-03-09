namespace Team121GBCapstoneProject.Models
{
    public class GameInfo
    {
        public List<Game> games { get; set; }
        public List<string> backgroundColor { get; set; }

        private string lightGreenHex = "#49c44d";
        private string yellowHex = "#e0c600";
        private string orangeHex = "#f2aa00";
        private string redHex = "#de2002";

        public List<string> colorSelection(List<Game> games)
        {
            List<string> selection = new List<string>();

            foreach (Game game in games)
            {
                if (game.AverageRating >= 8.0)
                {
                    selection.Add(lightGreenHex);
                }
                else if (game.AverageRating >= 6.0 && game.AverageRating <= 7.9)
                {
                    selection.Add(yellowHex);
                }
                else if (game.AverageRating >= 4.0 && game.AverageRating <= 5.9)
                {
                    selection.Add(orangeHex);
                }
                else
                {
                    selection.Add(redHex);
                }
            }


            return selection;
        } 
    }
}
