using System;
using NUnit.Framework;
using Standups_BDD_Tests.Drivers;
using Standups_BDD_Tests.PageObjects;
using Team121GB_BDD_Test.PageObjects;
using TechTalk.SpecFlow;

namespace Team121GB_BDD_Test.StepDefinitions
{
    [Binding]
    public class GP_87_CreateDefaultListsStepDefinitions
    {
        private readonly HomePageObject _homePage;
        private readonly LibraryPageObject _libraryPage;

        public GP_87_CreateDefaultListsStepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _homePage = new HomePageObject(browserDriver.Current);
            _libraryPage = new LibraryPageObject(browserDriver.Current);
        }
        [When(@"I click on the dropdown menu in the nav bar")]
        public void WhenIClickOnTheDropdownMenuInTheNavBar()
        {
            _homePage.NavbarBarDropDownLoggedIn.Click();
        }

        [When(@"I click on the Game Lists button in the navbar dropdown")]
        public void WhenIClickOnTheGameListsButtonInTheNavbarDropdown()
        {
            _homePage.GamesListButtonLoggedIn.Click();
        }

        [Then(@"I should be redirected to the '([^']*)'page")]
        public void ThenIShouldBeRedirectedToThePage(string pageName)
        {
            _libraryPage.GetTitle().Should().Be(pageName);
        }

        [Then(@"I should see my default lists display")]
        public void ThenIShouldSeeMyDefaultListsDisplay()
        {
            WhenIClickOnTheDropdownMenuInTheNavBar();
            WhenIClickOnTheGameListsButtonInTheNavbarDropdown();
            ThenIShouldBeRedirectedToThePage("Games Lists");
            _libraryPage.CurrentlyPlayingTable.Should().NotBeNull();
            _libraryPage.CompletedTable.Should().NotBeNull();
            _libraryPage.WantToPlayTable.Should().NotBeNull();
        }
    }
}
