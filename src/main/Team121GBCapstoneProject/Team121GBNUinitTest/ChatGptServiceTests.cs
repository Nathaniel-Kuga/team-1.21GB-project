using Team121GBCapstoneProject.Services.Concrete;
using Team121GBCapstoneProject.Models;
using Microsoft.Extensions.Configuration;
using OpenAI.GPT3;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;
using Moq;
using System.Net;

namespace Team121GBNUnitTest;
#nullable enable

public class ChatGptServiceTests
{
    private IConfigurationRoot _configuration;
  
    [SetUp]
    public void SetUp()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<ChatGptServiceTests>();
        _configuration = builder.Build();
    }

    [TestCase("", "")]
    [TestCase(null, "")]
    public void GetChatResponse(string prompt, string expectedResult)
    {
        // * Arrange
        string key = "Fake key";
        OpenAI.GPT3.OpenAiOptions openAiOptions = new OpenAI.GPT3.OpenAiOptions()
        {
            ApiKey = key,
            BaseDomain = "https://fake.com"
        };
        IOpenAIService openAiService = new OpenAIService(openAiOptions);
        ChatGptService chatGptService = new ChatGptService(openAiService);
        // ! Act
        string result = chatGptService.GetChatResponse(prompt).Result;
        // ? Assert
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void GetChatResponse_Success()
    {
        // * Arrange
        string key = _configuration["OpenAIServiceOptions:ApiKey"];
        string somePrompt = "Hello World!";
        OpenAI.GPT3.OpenAiOptions openAiOptions = new OpenAI.GPT3.OpenAiOptions()
        {
            ApiKey = key
        };
        IOpenAIService openAiService = new OpenAIService(openAiOptions);
        ChatGptService chatGptService = new ChatGptService(openAiService);
        // ! Act
        string result = chatGptService.GetChatResponse(somePrompt).Result;
        // ? Assert
        Assert.That(result, Is.Not.Null.And.Not.Empty.And.Not.EqualTo(somePrompt));

    }
}