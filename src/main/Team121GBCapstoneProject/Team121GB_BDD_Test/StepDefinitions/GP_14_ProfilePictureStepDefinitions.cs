using System;
using Standups_BDD_Tests.Drivers;
using Standups_BDD_Tests.PageObjects;
using Team121GB_BDD_Test.PageObjects;
using TechTalk.SpecFlow;

namespace Team121GB_BDD_Test.StepDefinitions
{
    [Binding]
    public class GP_14_ProfilePictureStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly LoginPageObject _loginPage;
        private readonly ProfilePageObject _profilePage;

        public GP_14_ProfilePictureStepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _loginPage = new LoginPageObject(browserDriver.Current);
            _profilePage = new ProfilePageObject(browserDriver.Current);
            _scenarioContext = context;
        }

        [Then(@"I should be able to select an image for my profile picture")]
        public void ThenIShouldBeAbleToSelectAnImageForMyProfilePicture()
        {
            _profilePage.profileUpload.Should().NotBeNull();
        }

        [When(@"I upload an image")]
        public void WhenIUploadAnImage()
        {
            _profilePage.UploadPhoto();
            _profilePage.Submit();
        }


    }
}
