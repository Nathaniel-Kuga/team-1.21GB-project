using Standups_BDD_Tests.PageObjects;
using System;
using Standups_BDD_Tests.Drivers;
using Standups_BDD_Tests.StepDefinitions;
using Team121GB_BDD_Test.PageObjects;
using TechTalk.SpecFlow;
using Microsoft.Extensions.Configuration;

namespace Team121GB_BDD_Test.StepDefinitions
{
    [Binding]
    public class GP_59_AiChatBotStepDefinitions
    {
        private readonly HomePageObject _homePage;
        private readonly AiChatBotPageObject _aiChatBotPage;
        private readonly UserLoginsStepDefinitions _userLoginsStepDefinitions;
        private readonly BrowserDriver _browserDriver;
        private readonly IConfiguration _configuration;

        public GP_59_AiChatBotStepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _homePage = new HomePageObject(browserDriver.Current);
            _aiChatBotPage = new AiChatBotPageObject(browserDriver.Current);
            _userLoginsStepDefinitions = new UserLoginsStepDefinitions(context, browserDriver);
            _browserDriver = browserDriver;
            IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<GP_85_SetDalleImageToProfilePictureStepDefinitions>();
            _configuration = builder.Build();
        }
        [Given(@"I am a visitor on the Home page")]
        public void GivenIAmAVisitorOnTheHomePage() =>_homePage.GoTo();

        [When(@"I try to navigate to the '([^']*)'")]
        public void WhenITryToNavigateToThe(string p0)
        {
            _homePage.GoTo("ChatGPT page");
            _browserDriver.Current.Title.Should().Be("Log in");
        }

        [Then(@"I should not see a button for the Chat bot page")]
        public void ThenIShouldNotSeeAButtonForTheChatBotPage() //=> _homePage.AiChatBotButtonLoggedIn.Should().BeNull();
        {
            int check = _homePage.CheckIfAiChatBotButtonExists();
            check.Should().Be(-1);
        }

        [Given(@"I am a logged in user")]
        public void GivenIAmALoggedInUser()
        {
            _homePage.GoTo();
            _userLoginsStepDefinitions.GivenIAmAUserWithFirstName("Talia");
            _userLoginsStepDefinitions.WhenILogin();
        }

        [When(@"I click on the chatbot button")]
        public void WhenIClickOnTheChatbotButton() => _homePage.AiChatBotButtonLoggedIn.Click();

            [Then(@"I should be redirected to the chatbot page")]
        public void ThenIShouldBeRedirectedToTheChatbotPage() => _browserDriver.Current.Title.Should().Be("ChatGPT page");

        [Given(@"I am on the chatbot page")]
        public void GivenIAmOnTheChatbotPage()
        {
            _aiChatBotPage.GoTo();
            _browserDriver.Current.Title.Should().Be("ChatGPT page");
        }

        [When(@"I enter a prompt")]
        public void WhenIEnterAPrompt() => _aiChatBotPage.InputPrompt("Hello World!");

        [When(@"I submit my prompt")]
        public void WhenISubmitMyPrompt() => _aiChatBotPage.SubmitPromptButton.Click();


        [Then(@"I should see a response to my prompt")]
        public void ThenIShouldSeeAResponseToMyPrompt()
        {
            int check = _aiChatBotPage.CheckIfResponseExists();
            check.Should().NotBe(-1).And.BeGreaterThanOrEqualTo(1);
        }

        [When(@"I enter an inappropriate prompt")]
        public void WhenIEnterAnInappropriatePrompt() => _aiChatBotPage.InputPrompt(_configuration["TerriblePrompt"]);


        [Then(@"I should be told my prompt was inappropriate")]
        public void ThenIShouldBeToldMyPromptWasInappropriate()
        {
            int check = _aiChatBotPage.CheckIfResponseForInappropriatePromptExists();
            check.Should().Be(1);
            _aiChatBotPage.SubmitPromptButton.Enabled.Should().BeFalse();
            _aiChatBotPage.PromptInput.Enabled.Should().BeFalse();
        }
    }
}
