using Standups_BDD_Tests.PageObjects;
using System;
using Standups_BDD_Tests.Drivers;
using Team121GB_BDD_Test.PageObjects;
using TechTalk.SpecFlow;

namespace Team121GB_BDD_Test.StepDefinitions
{
    [Binding]
    public class GP_193_WhisperIntegrationWithChatGptStepDefinitions
    {
        private readonly HomePageObject _homePage;
        private readonly AiChatBotPageObject _aiChatBotPage;

        public GP_193_WhisperIntegrationWithChatGptStepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _homePage = new HomePageObject(browserDriver.Current);
            _aiChatBotPage = new AiChatBotPageObject(browserDriver.Current);
        }   

        [When(@"I navigate to the chatbot page")]
        public void WhenINavigateToTheChatbotPage()
        {
            _homePage.AiChatBotButtonLoggedIn.Click();
        }

        [Then(@"I should see buttons for using speech to text\.")]
        public void ThenIShouldSeeButtonsForUsingSpeechToText_()
        {
            _aiChatBotPage.RecordAudioButton.Should().NotBeNull();
            _aiChatBotPage.StopRecordingButton.Should().NotBeNull();
        }
    }
}
