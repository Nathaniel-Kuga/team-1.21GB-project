namespace Team121GBCapstoneProject.Models.DTO
{
    public class GameJsonDTO
    {
        public int id { get; set; }
        public Cover cover { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string summary { get; set; }
        public int? first_release_date { get; set; }
        public double rating { get; set; }
        public List<AgeRating> age_ratings { get; set; }
        public List<IgdbGenre>? genres { get; set;}
        public List<IgdbPlatform>? platforms { get; set; }
        public static int? ConvertFirstReleaseDateFromUnixTimestampToYear(int? firstReleaseDate)
        {
            if (firstReleaseDate == null) return null;
            DateTime dateTime = new DateTime(1970,
                    1,
                    1,
                    0,
                    0,
                    0,
                    DateTimeKind.Utc)
                .AddSeconds((double) firstReleaseDate);
            return dateTime.Year;
        }

        public static int? ExtractEsrbRatingFromAgeRatingsArray(List<AgeRating> ageRatings)
        {
            int? rating = null;
            if (ageRatings != null) rating = ageRatings.FirstOrDefault(ageRating => ageRating.category == 1)?.rating;
            return rating;
        }
    }

    public class Cover
    {
        public int id { get; set; }
        public string url { get; set; }
    }

    public class AgeRating
    {
        public int? rating { get; set; }
        public int? category { get; set; }
    }

    public class IgdbGenre 
    {
        public string name { get; set; }
    }

    public class IgdbPlatform
    {
        public string name { get; set; }
    }
}