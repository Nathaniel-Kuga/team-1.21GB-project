/*
 * These tests were written by Nathaniel Kuga on 4/6/23
 * If you change or add tests, please note where you make changes
 * Or note which tests you wrote.
 */

using Team121GBCapstoneProject.Models.DTO;

namespace Team121GBNUnitTest;

public class GameJsonDTOTests
{
   
    [TestCase(10223123, 1970)]// 10223123 == 1970
    [TestCase(1000000000, 2001)]// 10223123 == 2001
    [TestCase(1649266800, 2022)]// 1649266800 == 2022
    [TestCase(null, null)]// should be null if the unixTimeStamp is null
    public void GameJsonDTO_ConvertFirstReleaseDate_FromUnixTimeStampToInteger(int? unixTimeStamp, int? expectedValue)
    {
        // Arrange  and Act
        int? yearPublished = GameJsonDTO.ConvertFirstReleaseDateFromUnixTimestampToYear(unixTimeStamp);
        //Assert
        Assert.That(yearPublished, Is.EqualTo(expectedValue));
    }


    [Test]
    [TestCaseSource(nameof(AgeRatingWithEsrbRatings))]
    [TestCaseSource(nameof(AgeRatingWithoutEsrbRatings))]
    [TestCaseSource(nameof(NullResponse))]
    public void GameJsonDTO_ExtractEsrbRatingFromAgeRatingsArray(List<AgeRating>? ageRatings, int? expectedAgeRatings)
    {
        //Arrange and Act
        int? result = GameJsonDTO.ExtractEsrbRatingFromAgeRatingsArray(ageRatings);
        //Assert
        Assert.That(result, Is.EqualTo(expectedAgeRatings));
    }

    private static readonly object[] AgeRatingWithEsrbRatings =
    {
        new object[]
        {
            new List<AgeRating>
            {
                new AgeRating{ rating = 10, category = 1},
                new AgeRating{ rating = 4, category = 2},
                new AgeRating{ rating = 14,  category = 3}
            },
            10
        }
    };
    private static readonly object[] AgeRatingWithoutEsrbRatings =
    {
        new object[]
        {
            new List<AgeRating>
            {
                new AgeRating{ rating = 4, category = 2},
                new AgeRating{ rating = 14,  category = 3}
            },
            null
        }
    };
    private static readonly object[] NullResponse =
    {
        new object[]
        {
            null,
            null
        }
    };
}