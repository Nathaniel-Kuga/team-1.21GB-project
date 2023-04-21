
using System;
using Standups_BDD_Tests.Drivers;
using Standups_BDD_Tests.PageObjects;
using Standups_BDD_Tests.StepDefinitions;
using Team121GB_BDD_Test.PageObjects;
using TechTalk.SpecFlow;

namespace Team121GB_BDD_Test.StepDefinitions
{
    [Binding]
    public class GP_34_ViewTop100GamesStepDefinitions
    {
        private readonly HomePageObject _homePage;
        private readonly Top100GamesPageObject _top100GamesPage;
        
        public GP_34_ViewTop100GamesStepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _homePage = new HomePageObject(browserDriver.Current);
            _top100GamesPage = new Top100GamesPageObject(browserDriver.Current);
        }
        [When(@"When I click the ""([^""]*)"" button")]
        public void WhenWhenIClickTheTopGamesPageButton(string pageName)
        {
            _homePage.GoTo();
            _homePage.Top100GamesButton.Click();
        }

        [Then(@"I should be redirected to the ""([^""]*)""")]
        public void ThenIShouldBeRedirectedToTheTopGamesPage(string pageName)
        {
            _top100GamesPage.GetTitle().Should().Be(pageName);
        }
    }
}
