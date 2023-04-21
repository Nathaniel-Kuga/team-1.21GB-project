using Standups_BDD_Tests.Drivers;
using Standups_BDD_Tests.PageObjects;
using System;
using Standups_BDD_Tests.StepDefinitions;
using Team121GB_BDD_Test.PageObjects;
using TechTalk.SpecFlow;

namespace Team121GB_BDD_Test.StepDefinitions
{
    [Binding]
    public class GP_18StepDefinitions
    {

        private readonly ScenarioContext _scenarioContext;
        private readonly LoginPageObject _loginPage;
        private readonly ProfilePageObject _profilePage;
        
        public GP_18StepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _loginPage = new LoginPageObject(browserDriver.Current);
            _profilePage = new ProfilePageObject(browserDriver.Current);
            _scenarioContext = context;
        }

        [When(@"I navigate to the '([^']*)' page")]
        public void WhenINavigateToThePage(string page)
        {
            _profilePage.GoTo();
        }



        [Then(@"I should see a form with a text box to edit my profile name and a submit button\.")]
        public void ThenIShouldSeeAFormWithATextBoxToEditMyProfileNameAndASubmitButton_()
        {
            _profilePage.firstNameBox.Should().NotBeNull();
            _profilePage.lastNameBox.Should().NotBeNull();
            _profilePage.firstNameBox.Displayed.Should().BeTrue();
            _profilePage.lastNameBox.Displayed.Should().BeTrue();
        }

        [When(@"the I type in valid input")]
        public void WhenTheITypeInValidInput()
        {
            TestUser u = (TestUser) _scenarioContext["CurrentUser"];
            _profilePage.firstNameBox.Clear();
            _profilePage.firstNameBox.SendKeys(u.FirstName);
            _profilePage.lastNameBox.Clear();
            _profilePage.lastNameBox.SendKeys(u.LastName);
        }

        [When(@"I submit the form for editing my profile name\.")]
        public void WhenISubmitTheFormForEditingMyProfileName_()
        {
            _profilePage.Submit();
        }


        [Then(@"I should see the change reflected on page reload\.")]
        public void ThenIShouldSeeTheChangeReflectedOnPageReload_()
        {
            TestUser u = (TestUser)_scenarioContext["CurrentUser"];
            _profilePage.NavbarWelcomeText().Should().ContainEquivalentOf(u.FirstName, AtLeast.Once());
        }


        [When(@"then I type in invalid input")]
        public void WhenThenITypeInInvalidInput()
        {
            _profilePage.firstNameBox.Clear();
            _profilePage.lastNameBox.Clear();
        }

        [Then(@"I should see a notification telling my the input is invalid\.")]
        public void ThenIShouldSeeANotificationTellingMyTheInputIsInvalid_()
        {
            _profilePage.FindErrorText();

            _profilePage.firstNameError.Displayed.Should().Be(true);
            _profilePage.lastNameError.Displayed.Should().Be(true);
        }

    }

}
