using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Contrib.HttpClient;
using Team121GBCapstoneProject.Models;
using Team121GBCapstoneProject.DAL.Abstract;
using Team121GBCapstoneProject.DAL.Concrete;
using Team121GBCapstoneProject.Services.Concrete;
using Team121GBCapstoneProject.Models;
using Team121GBNUnitTest;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Moq.Protected;

namespace Team121GBNUnitTest;

public class IgdbAPIServiceTests
{
    private string _igdbClientId;
    private string _igdbBearerToken;
    private IConfigurationRoot _configuration;

    private IGameRepository _gameRepository;
    private IRepository<Game> _genericGameRepo;
    private IRepository<Esrbrating> _esrbratingRepo;
    private InMemoryDbHelper<GPDbContext> _dbHelper;
    private IRepository<GameGenre> _gameGenreRepository;
    private IRepository<Genre> _genreRepository;
    private IRepository<Platform> _platformRepository;
    private IRepository<GamePlatform> _gamePlatformRepository;
    private string _search1;
    private string _search2;
    private string _search3;
    private string _platform1;
    private string _platform2;
    private string _platform3;
    private string _genre1;
    private string _genre2;
    private string _genre3;
    private int _esrbRatingId1;
    private int _esrbRatingId2;
    private int _esrbRatingId3;
    [SetUp]
    public void SetUp()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<IgdbAPIServiceTests>();
        _configuration = builder.Build();
        _igdbClientId = _configuration["GamingPlatform:igdbClientId"];
        _igdbBearerToken = _configuration["GamingPlatform:igdbBearerToken"];
        _dbHelper = new InMemoryDbHelper<GPDbContext>(null, DbPersistence.OneDbPerTest);
        _gameRepository = new GameRepository(_dbHelper.GetContext());
        _esrbratingRepo = new Repository<Esrbrating>(_dbHelper.GetContext());
        _genericGameRepo = new Repository<Game>(_dbHelper.GetContext());
        _gameGenreRepository = new Repository<GameGenre>(_dbHelper.GetContext());
        _genreRepository = new Repository<Genre>(_dbHelper.GetContext());
        _platformRepository = new Repository<Platform>(_dbHelper.GetContext());
        _gamePlatformRepository = new Repository<GamePlatform>(_dbHelper.GetContext());

        _search1 = "Mario";
        _search2 = "Zelda";
        _search3 = "Sonic";
    }

    // Tests from Moshe (Sprint 2 & 3)

    [Test]
    public async Task IgdbService_GetJsonStringFromEndpoint_RequestAccepted()
    {
        // --> Arrange
        // Define the search query and the endpoint URL
        string searchBody = $"search \"{_search1}\"; fields name, cover.url, url; where parent_game = null;";
        string searchUri = "https://api.igdb.com/v4/games/";

        // Create a mock HttpMessageHandler to handle the request
        var handler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"key\":\"value\"}") });

        // Use the mock handler to create an HttpClient that will return the mock response
        var httpClient = new HttpClient(handler.Object)
        {
            BaseAddress = new Uri("https://api.igdb.com/v4/")
        };

        // Create a mock IHttpClientFactory that will return the mock HttpClient
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Create an instance of the IgdbService, passing in the mock HttpClientFactory and any other dependencies
        var igdbService = new IgdbService(mockHttpClientFactory.Object, _gameRepository, _genericGameRepo, _esrbratingRepo, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository);
        // --> Act
        // Call the method under test and capture the result
        var result = await igdbService.GetJsonStringFromEndpoint(_igdbBearerToken, searchUri, _igdbClientId, searchBody);

        // --> Assert
        // Verify that the result matches the expected value
        Assert.That(result, Is.EqualTo("{\"key\":\"value\"}"));
    }

    [Test]
    public async Task IgdbService_GetJsonStringFromEndpoint_RequestDenied_ThrowsException()
    {
        // --> Arrange
        // Define the search request body and URI
        string searchBody = $"search \"{_search1}\"; fields name, cover.url, url; where parent_game = null;";
        string searchUri = "https://api.igdb.com/v4/games/";

        // Set up a mock HttpMessageHandler with a strict mock behavior
        var handler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        // Set up the mock handler to return a Forbidden response for any request message
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Forbidden));

        // Create a new HttpClient using the mock handler, and set the BaseAddress
        var httpClient = new HttpClient(handler.Object)
        {
            BaseAddress = new Uri("https://api.igdb.com/v4/")
        };

        // Set up a mock IHttpClientFactory that returns the mock HttpClient
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Create a new instance of the IgdbService using the mock HttpClientFactory
        var igdbService = new IgdbService(mockHttpClientFactory.Object, _gameRepository, _genericGameRepo, _esrbratingRepo, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository);

        // --> Act & Assert
        // Assert that calling the method with the mock HttpClient will throw an HttpRequestException
        Assert.ThrowsAsync<System.Net.Http.HttpRequestException>(async () =>
            await igdbService.GetJsonStringFromEndpoint(_igdbBearerToken, searchUri, _igdbClientId, searchBody)
        );
    }

   /* [Test]
    public async Task IgdbService_SearchGames_ReturnsListOf10Games()
    {
        // --> Arrange
        // Set up a mock HttpClientFactory that returns a HttpClient with a custom validation callback for the SSL certificate
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(() =>
        {
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            return new HttpClient(handler)
            {
                BaseAddress = new Uri("https://api.igdb.com/v4/")
            };
        });

        // Instantiate a new instance of the IgdbService class and pass in the mocked HttpClientFactory
        var igdbService = new IgdbService(mockHttpClientFactory.Object, _gameRepository, _genericGameRepo, _esrbratingRepo, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository);

        // Set the credentials needed to access the IGDB API
        ((Team121GBCapstoneProject.Services.Abstract.IIgdbService)igdbService).SetCredentials(_igdbClientId, _igdbBearerToken);

        // --> Act
        // Call the SearchGames method with three different search queries and capture the results in variables
        var search1Games = await ((Team121GBCapstoneProject.Services.Abstract.IIgdbService)igdbService).SearchGames(_search1);
        var search2Games = await ((Team121GBCapstoneProject.Services.Abstract.IIgdbService)igdbService).SearchGames(_search2);
        var search3Games = await ((Team121GBCapstoneProject.Services.Abstract.IIgdbService)igdbService).SearchGames(_search3);

        // --> Assert
        // Check that each of the three search results contains exactly 10 games
        Assert.That(search1Games.Count(), Is.EqualTo(10));
        Assert.That(search2Games.Count(), Is.EqualTo(10));
        Assert.That(search3Games.Count(), Is.EqualTo(10));
    }

    [Test]
    public async Task IgdbService_SearchGames_EmptySearchReturnsEmpty()
    {
        // --> Arrange
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        var httpClient = new HttpClient();
        mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
        var igdbService = new IgdbService(mockHttpClientFactory.Object, _gameRepository, _genericGameRepo, _esrbratingRepo, _gameGenreRepository, _genreRepository, _gamePlatformRepository, _platformRepository);


        ((Team121GBCapstoneProject.Services.Abstract.IIgdbService)igdbService).SetCredentials(_igdbClientId, _igdbBearerToken);

        // --> Act
        var result = await ((Team121GBCapstoneProject.Services.Abstract.IIgdbService)igdbService).SearchGames("");

        // --> Assert
        Assert.IsEmpty(result);
    }
*/

    //[Test]
    //public async Task IgdbService_Request_Denied()
    //{
    //    //Arrange
    //    var handler = new Mock<HttpMessageHandler>();
    //    string searchBody = $"search \"Mario\"; fields name, cover.url, url; where parent_game = null;";
    //    string searchUri = "https://api.igdb.com/v4/games/";

    //    handler.SetupAnyRequest()
    //        .ReturnsResponse(HttpStatusCode.NotFound);
    //    //New up the service class
    //    IgdbService igdbService = new IgdbService(handler.CreateClientFactory(), _gameRepository, _genericGameRepo);

    //    //Act
    //    Task<string> Act() => igdbService.GetJsonStringFromEndpoint("incorrectToken", searchUri, _igdbClientId, searchBody);

    //    //Assert
    //    Assert.That(Act, Throws.TypeOf<HttpRequestException>());
    //}
}
