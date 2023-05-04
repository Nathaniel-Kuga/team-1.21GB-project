using OpenQA.Selenium;
using Standups_BDD_Tests.Drivers;
using Standups_BDD_Tests.PageObjects;
using System;
using Team121GB_BDD_Test.PageObjects;
using TechTalk.SpecFlow;

namespace Team121GB_BDD_Test.StepDefinitions
{
    [Binding]
    public class GP_63_SpeedSearchStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly HomePageObject _homePage;
        private readonly SpeedSearchPageObject _speedSearchPageObject;
        public GP_63_SpeedSearchStepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _homePage = new HomePageObject(browserDriver.Current);
            _speedSearchPageObject = new SpeedSearchPageObject(browserDriver.Current);
            _scenarioContext = context;
        }

  /*      [Given(@"I am redirected to the '([^']*)' page")]
        public void GivenIAmRedirectedToThePage(string home)
        {
            _homePage.GetTitle().Should().ContainEquivalentOf(home, AtLeast.Once());
        }*/

        [When(@"I click the profile dropdown"), Given(@"I click the profile dropdown")]
        public void WhenIClickTheProfileDropdown()
        {
            _homePage.OpenUserDropdown();
        }

        [Then(@"I will see a item named SpeedSearch")]
        public void ThenIWillSeeAItemNamedSpeedSearch()
        {
            _homePage.SpeedSearchButtonLoggedIn.Displayed.Should().BeTrue();
        }

  /*      [Given(@"I click the profile dropdown")]
        public void GivenIClickTheProfileDropdown()
        {
            throw new PendingStepException();
        }*/

        [When(@"I click SpeedSearch")]
        public void WhenIClickSpeedSearch()
        {
            _homePage.ClickOnSpeedSearch();
        }

        [Then(@"I will go to the SpeedSearch Page")]
        public void ThenIWillGoToTheSpeedSearchPage()
        {
            _speedSearchPageObject.GetTitle().Should().ContainEquivalentOf("Speed Search", AtLeast.Once());
        }

        [Given(@"I am on the SpeedSearch Page")]
        public void GivenIAmOnTheSpeedSearchPage()
        {
            _speedSearchPageObject.GoTo();
        }

        [When(@"enter in a blank list")]
        public void WhenEnterInABlankList()
        {
            string blankList = "";
            _speedSearchPageObject.EnterGames(blankList);
        }

        [When(@"click the submit button")]
        public void WhenClickTheSubmitButton()
        {
            _speedSearchPageObject.SubmitGames();
        }

        [Then(@"I will be shown a error")]
        public void ThenIWillBeShownAError()
        {
            _speedSearchPageObject.HasGameErrors().Should().BeTrue();
        }

        [When(@"I enter in valid data")]
        public void WhenIEnterInValidData()
        {
            string game = "*Super Mario 64 *Halo: Combat Evolved";
            _speedSearchPageObject.EnterGames(game);
        }

        [Then(@"I will go the the Results page")]
        public void ThenIWillGoTheTheResultsPage()
        {
            _speedSearchPageObject.GetTitle().Should().ContainEquivalentOf("Speed Search Result", AtLeast.Once());
        }
    }
}
