using Standups_BDD_Tests.Drivers;
using Standups_BDD_Tests.PageObjects;
using System;
using Team121GB_BDD_Test.PageObjects;
using TechTalk.SpecFlow;

namespace Team121GB_BDD_Test.StepDefinitions
{
    [Binding]
    public class GP_157_LookUpOtherUsersStepDefinitions
    {

        private readonly ScenarioContext _scenarioContext;
        private readonly LoginPageObject _loginPage;
        private readonly ProfilePageObject _profilePage;
        private readonly FriendPageObject _friendPage;

        public GP_157_LookUpOtherUsersStepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _loginPage = new LoginPageObject(browserDriver.Current);
            _profilePage = new ProfilePageObject(browserDriver.Current);
            _friendPage = new FriendPageObject(browserDriver.Current);
            _scenarioContext = context;
        }

        [Then(@"I should see a button to find users")]
        public void ThenIShouldSeeAButtonToFindUsers()
        {
            _profilePage.findFriendsBtn.Should().NotBeNull();
            _profilePage.findFriendsBtn.Displayed.Should().BeTrue();
        }

        [Then(@"I click on it it will take me to the ""([^""]*)"" page")]
        public void ThenIClickOnItItWillTakeMeToThePage(string p0)
        {
            _profilePage.findFriendsBtn.Click();
            _profilePage.GetURL().Should().ContainEquivalentOf(p0, AtLeast.Once());
        }


        [When(@"I look up a valid user")]
        public void WhenILookUpAValidUser()
        {
            _friendPage.InputEmail.SendKeys("BDDTesting1@gmail.com");
            _friendPage.FriendButton.Click();
        }

        [Then(@"I will see their information")]
        public void ThenIWillSeeTheirInformation()
        {
            _friendPage.Picture.Should().NotBeNull();
            _friendPage.Picture.Displayed.Should().BeTrue();
        }

        [When(@"I look up an invalid user")]
        public void WhenILookUpAnInvalidUser()
        {
            _friendPage.InputEmail.SendKeys("ThisIsNotAnEmail@NotAnEmail.NotCom");
            _friendPage.FriendButton.Click();
        }

        [Then(@"I will see an ""([^""]*)"" error")]
        public void ThenIWillSeeAnError(string p0)
        {
            _friendPage.NotFoundError.Should().NotBeNull();
            _friendPage.NotFoundError.Displayed.Should().BeTrue();
            _friendPage.NotFoundError.Text.Should().Be(p0);
        }

    }
}
