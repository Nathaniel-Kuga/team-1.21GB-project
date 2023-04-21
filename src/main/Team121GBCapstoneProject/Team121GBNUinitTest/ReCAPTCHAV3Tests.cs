using Moq;
using Moq.Contrib.HttpClient;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Team121GBCapstoneProject.Services.Concrete;

namespace Team121GBNUnitTest;

public class ReCAPTCHAV3Tests
{
    [SetUp]
    public void SetUp()
    {

    }

    [Test]
    public void ReCaptchaV3_Success_ShouldReturnTrue()
    {
        // * Arrange
        string secretKey = "SecretKey";
        string captcha = "captcha_response";
        string responseJson = "{\"success\": true}";
        string url = "https://fake.com";

        var handler = new Mock<HttpMessageHandler>();
        HttpResponseMessage response = new HttpResponseMessage()
        {
            Content = new StringContent(responseJson)
        };
        handler.SetupAnyRequest().ReturnsAsync(response);


        ReCaptchaV3Service reCaptchaV3 = new ReCaptchaV3Service(secretKey, handler.CreateClientFactory());

        // ! Act
        bool result = reCaptchaV3.IsValid(captcha, url).Result;

        // ? Assert
        Assert.That(result, Is.EqualTo(true));
    }
    [Test]
    public void ReCaptchaV3_Failure_ShouldReturnFalse()
    {
        // * Arrange
        string secretKey = "SecretKey";
        string captcha = "captcha_response";
        string responseJson = "{\"success\": false}";
        string url = "https://fake.com";
        
        var handler = new Mock<HttpMessageHandler>();
        HttpResponseMessage response = new HttpResponseMessage()
        {
            Content = new StringContent(responseJson)
        };
        handler.SetupAnyRequest().ReturnsAsync(response);

        ReCaptchaV3Service reCaptchaV3 = new ReCaptchaV3Service(secretKey, handler.CreateClientFactory());
        // ! Act
        bool result = reCaptchaV3.IsValid(captcha, url).Result;

        // ? Assert
        Assert.That(result, Is.EqualTo(false));
    }
    [Test]
    public void ReCaptchaV3_NotFound_ShouldReturnFalse() 
    {
        // * Arrange
        string secretKey = "SecretKey";
        string captcha = "captcha_response";
        string responseJson = "{}";
        string url = "https://fake.com";

        var handler = new Mock<HttpMessageHandler>();
        HttpResponseMessage response = new HttpResponseMessage()
        {
            Content = new StringContent(responseJson)
        };
        handler.SetupAnyRequest().ReturnsAsync(response);


        ReCaptchaV3Service reCaptchaV3 = new ReCaptchaV3Service(secretKey, handler.CreateClientFactory());
        
        // ! Act
        bool result = reCaptchaV3.IsValid(captcha, url).Result;

        // ? Assert
        Assert.That(result, Is.EqualTo(false));
    }
}
