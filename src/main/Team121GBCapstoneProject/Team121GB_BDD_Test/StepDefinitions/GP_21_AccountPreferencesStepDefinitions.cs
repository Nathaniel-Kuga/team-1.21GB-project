using System;
using Standups_BDD_Tests.Drivers;
using Standups_BDD_Tests.PageObjects;
using Standups_BDD_Tests.StepDefinitions;
using Team121GB_BDD_Test.PageObjects;
using TechTalk.SpecFlow;

namespace Team121GB_BDD_Test.StepDefinitions
{
    [Binding]
    public class GP_21_AccountPreferencesStepDefinitions
    {
        private readonly HomePageObject _homePage;
        private readonly LoginPageObject _loginPage;
        private readonly ProfilePageObject _profilePage;
        private readonly ScenarioContext _scenarioContext;
        private readonly UserLoginsStepDefinitions _userLoginsStepDefinitions;
        public GP_21_AccountPreferencesStepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _loginPage = new LoginPageObject(browserDriver.Current);
            _homePage = new HomePageObject(browserDriver.Current);
            _profilePage = new ProfilePageObject(browserDriver.Current);
            _userLoginsStepDefinitions = new UserLoginsStepDefinitions(context, browserDriver);
            _scenarioContext = context;
        }

        [Given(@"I am a logged in user with Firstname '([^']*)'")]
        public void GivenIAmALoggedInUserWithFirstname(string firstName)
        {
            _homePage.GoTo();
            _userLoginsStepDefinitions.GivenIAmAUserWithFirstName(firstName);
            _userLoginsStepDefinitions.WhenILogin();
        }
        
        [Given(@"I am on the '([^']*)' page")]
        public void GivenIAmOnThePage(string home)
        {
            // Redirected here from logging in
        }

        [When(@"I click my name in the navbar")]
        public void WhenIClickMyNameInTheNavbar()
        {
            _homePage.NavBarHelloLink.Click();
        }

        [Then(@"I should be redirected to the '([^']*)' page")]
        public void ThenIShouldBeRedirectedToThePage(string profile)
        {
            _profilePage.GetTitle().Should().Be(profile);
        }
    }
}
