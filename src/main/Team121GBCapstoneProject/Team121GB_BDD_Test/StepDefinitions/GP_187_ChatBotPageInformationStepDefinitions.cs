using Standups_BDD_Tests.PageObjects;
using System;
using Standups_BDD_Tests.Drivers;
using Team121GB_BDD_Test.PageObjects;
using TechTalk.SpecFlow;

namespace Team121GB_BDD_Test.StepDefinitions;
[Binding]
public class GP_187_ChatBotPageInformationStepDefinitions
{
    private readonly HomePageObject _homePage;
    private readonly AiChatBotPageObject _aiChatBotPage;

    public GP_187_ChatBotPageInformationStepDefinitions (ScenarioContext context, BrowserDriver browserDriver)
    {
        _homePage = new HomePageObject(browserDriver.Current);
        _aiChatBotPage = new AiChatBotPageObject(browserDriver.Current);
    }

    [Then(@"I should see a description of what the page is for")]
    public void ThenIShouldSeeADescriptionOfWhatThePageIsFor()
    {
        _aiChatBotPage.DescriptAndInformationCard.Should().NotBeNull();
    }

    [Then(@"I should see information about the limitations of the chat bot")]
    public void ThenIShouldSeeInformationAboutTheLimitationsOfTheChatBot()
    {
        _aiChatBotPage.DescriptAndInformationCard.Should().NotBeNull();
    }
}