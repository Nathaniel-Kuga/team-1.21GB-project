using Standups_BDD_Tests.Drivers;
using Standups_BDD_Tests.PageObjects;
using Standups_BDD_Tests.Shared;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Standups_BDD_Tests.StepDefinitions
{
    // Wrapper for the data we get for each user
    public class TestUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }

    [Binding]
    public class UserLoginsStepDefinitions
    {
        // The context is shared between all step definition files.
        // This is where we put data that is shared between steps in different files.
        private readonly ScenarioContext _scenarioContext;
        private readonly LoginPageObject _loginPage;
        private readonly HomePageObject _homePage;

        public UserLoginsStepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _loginPage = new LoginPageObject(browserDriver.Current);
            _homePage = new HomePageObject(browserDriver.Current);
            _scenarioContext = context;
        }

        [Given(@"the following users exist")]
        public void GivenTheFollowingUsersExist(Table table)
        {
            // Nothing to do for this step other than to save the background data
            // that defines the users
            IEnumerable<TestUser> users = table.CreateSet<TestUser>();
            _scenarioContext["Users"] = users;
        }

        [Given(@"the following users do not exist")]
        public void GivenTheFollowingUsersDoNotExist(Table table)
        {
            // Same with this one, just setting up the background data
            IEnumerable<TestUser> nonUsers = table.CreateSet<TestUser>();
            _scenarioContext["NonUsers"] = nonUsers;
        }

        [Given(@"I am a user with first name '([^']*)'")]
        public void GivenIAmAUserWithFirstName(string firstName)
        {
            // Find this user, first look in users, then in non-users
            IEnumerable<TestUser> users = (IEnumerable<TestUser>)_scenarioContext["Users"];
            TestUser u = users.Where(u => u.FirstName == firstName).FirstOrDefault();
            if (u == null)
            {
                // must have been selecting from non-users
                IEnumerable<TestUser> nonUsers = (IEnumerable<TestUser>)_scenarioContext["NonUsers"];
                u = nonUsers.Where(u => u.FirstName == firstName).FirstOrDefault();
            }
            _scenarioContext["CurrentUser"] = u;
        }

        [When(@"I login"), Given(@"I login")]
        public void WhenILogin()
        {
            // Go to the login page
            _loginPage.GoTo();
            // Now (attempt to) log them in.  Assumes previous steps defined the user
            TestUser u = (TestUser)_scenarioContext["CurrentUser"];
            _loginPage.EnterEmail(u.Email);
            _loginPage.EnterPassword(u.Password);
            _loginPage.Login();
        }

        [Then(@"I am redirected to the '([^']*)' page")]
        public void ThenIAmRedirectedToThePage(string pageName)
        {
            // how do we identify which page we're on?  We're saving that in Common, so use that data
            // any page object will do
            _loginPage.GetURL().Should().Be(Common.UrlFor(pageName));
        }

        [Then(@"I can see a personalized message in the navbar that includes my email")]
        public void ThenICanSeeAPersonalizedMessageInTheNavbarThatIncludesMyEmail()
        {
            // This is after a redirection to the homepage so we need to use that page
            TestUser u = (TestUser)_scenarioContext["CurrentUser"];
            _homePage.NavbarWelcomeText().Should().ContainEquivalentOf(u.FirstName, AtLeast.Once());
        }

        [Then(@"I can see a login error message")]
        public void ThenICanSeeALoginErrorMessage()
        {
            _loginPage.HasLoginErrors().Should().BeTrue();
        }

    }
}