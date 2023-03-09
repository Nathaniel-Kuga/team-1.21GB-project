using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Moq;
using Moq.Contrib.HttpClient;  
using Team121GBCapstoneProject.Services;
using Team121GBCapstoneProject.Models;

namespace Team121GBNUinitTests;

public class IgdbAPIServiceTests
{
    [SetUp]
    public void SetUp()
    {
    }

    [Test]
    public void IgdbService_Request_Success()
    {
        //Arrange
        var handler = new Mock<HttpMessageHandler>();

        handler.SetupAnyRequest()
               .ReturnsResponse(HttpStatusCode.OK);
        
        //New up the service class
        var igdbService = new IgdbService(handler.CreateClientFactory());
        //Act
        

        //Assert
    }
}