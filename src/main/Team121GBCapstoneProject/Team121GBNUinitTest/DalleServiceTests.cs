using Team121GBCapstoneProject.Services.Concrete;
using Team121GBCapstoneProject.Models;
using Microsoft.Extensions.Configuration;
using OpenAI.GPT3;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;

namespace Team121GBNUnitTest;
#nullable enable
public class DalleServiceTests
{
    private IConfigurationRoot _configuration;
    private string _dallePrivateKey;
    private string _dallePublicKey;
    private IOpenAIService _openAiService;
    private InMemoryDbHelper<GPDbContext> _dbHelper;

    [SetUp]
    public void Setup()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<DalleServiceTests>();
        _configuration = builder.Build();
    }

    [TestCase("", null)]
    [TestCase(null, null)]
    public void TurnImageUrlIntoByteArray(string imageURL, byte[] expectedResult)
    {
        // * Arrange
        string key = _configuration["OpenAIServiceOptions:ApiKey"];
        OpenAIService openAiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = key
        });
        HttpClient httpClient = new HttpClient();
        DalleService dalleService = new DalleService(openAiService, httpClient);
        // ! Act
        byte[] result = dalleService.TurnImageUrlIntoByteArray(imageURL).Result;
        // ? Assert
        Assert.That(result, Is.EqualTo(expectedResult));
    }
    [Test]
    public void TurnImageUrlIntoByteArraySuccess()
    {
        // * Arrange
        string key = _configuration["OpenAIServiceOptions:ApiKey"];
        OpenAIService openAiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = key
        });
        HttpClient httpClient = new HttpClient();
        DalleService dalleService = new DalleService(openAiService, httpClient);
        string imageURL = "https://content.codecademy.com/courses/web-101/web101-image_brownbear.jpg";
        // ! Act
        byte[] result = dalleService.TurnImageUrlIntoByteArray(imageURL).Result;
        // ? Assert
        Assert.That(result, Is.Not.Null.And.Not.Empty);
    }
}