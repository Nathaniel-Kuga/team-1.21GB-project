using Newtonsoft.Json;

namespace Team121GBCapstoneProject.Models
{
    public class IgdbGame
    {
        public int? Id { get; set; }
        public string GameTitle { get; set; }
        public string GameCoverArt { get; set; } 
        public string GameWebsite { get; set; }
        public string GameDescription { get; set; }
        public int? FirstReleaseDate { get; set; }
        public double? AverageRating { get; set; }
        public int? ESRBRatingValue { get; set; }
        // public int IgdbId { get; set; }
        public List<string>? Genres { get; set;}
        public List<string>? Platforms { get; set; }

        public IgdbGame(int? id,
                        string gameTitle,
                        string gameCoverArt,
                        string gameWebsite,
                        string gameDescription,
                        int? firstReleaseDate,
                        double? averageRating,
                        int? eSRBRatingValue)
        {
            Id = id;
            GameTitle = gameTitle;
            GameCoverArt = gameCoverArt;
            GameWebsite = gameWebsite;
            GameDescription = gameDescription;
            FirstReleaseDate = firstReleaseDate;
            AverageRating = averageRating;
            ESRBRatingValue = eSRBRatingValue;
        }

        // * new updated constructor for improved caching.
        public IgdbGame(int? id,
                        string gameTitle,
                        string gameCoverArt,
                        string gameWebsite,
                        string gameDescription,
                        int? firstReleaseDate,
                        double? averageRating,
                        int? eSRBRatingValue,
                        // int igdbId,
                        List<string>? genres,
                        List<string>? platforms)
        {
            Id = id;
            GameTitle = gameTitle;
            GameCoverArt = gameCoverArt;
            GameWebsite = gameWebsite;
            GameDescription = gameDescription;
            FirstReleaseDate = firstReleaseDate;
            AverageRating = averageRating;
            ESRBRatingValue = eSRBRatingValue;
            // IgdbId = igdbId;
            Genres = genres;
            Platforms = platforms;
        }
    }
}
