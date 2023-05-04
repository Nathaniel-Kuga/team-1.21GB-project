using Standups_BDD_Tests.Drivers;
using Standups_BDD_Tests.PageObjects;
using System;
using TechTalk.SpecFlow;

namespace Team121GB_BDD_Test.StepDefinitions
{
    [Binding]
    public class GP_64_SteamGamesStepDefinitions
    {
        private readonly HomePageObject _homePage;
        public GP_64_SteamGamesStepDefinitions(BrowserDriver browserDriver)
        {
            _homePage = new HomePageObject(browserDriver.Current);
        }
        [When(@"I click on the Steam Games button in the navbar dropdown")]
        public void WhenIClickOnTheSteamGamesButtonInTheNavbarDropdown()
        {
            _homePage.SteamGamesButtonLoggedIn.Click();
        }
    }
}
