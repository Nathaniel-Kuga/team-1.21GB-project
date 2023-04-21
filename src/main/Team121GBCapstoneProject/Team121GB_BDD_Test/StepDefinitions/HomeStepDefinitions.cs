using AspNetCore;
using NUnit.Framework;
using Standups_BDD_Tests.Drivers;
using Standups_BDD_Tests.PageObjects;
using System;
using TechTalk.SpecFlow;

namespace Team121GB_BDD_Test.StepDefinitions
{
    [Binding]
    public class HomeStepDefinitions
    {
        private readonly HomePageObject _homePage;
        public HomeStepDefinitions(BrowserDriver browserDriver) 
        {
            _homePage = new HomePageObject(browserDriver.Current);
        }    

        [Given(@"I am a visitor")]
        public void GivenIAmAVisitor()
        {
            //Nothing to Do!!
        }

        [Given(@"I am on the ""([^""]*)"" page"), When(@"I am on the ""([^""]*)"" page")]
        public void WhenIAmOnThePage(string pageName)
        {
            _homePage.GoTo(pageName);
        }

        [Then(@"The page title contains ""([^""]*)""")]
        public void ThenThePageTitleContains(string p0)
        {
            _homePage.GetTitle().Should().ContainEquivalentOf(p0, AtLeast.Once());
        }

        [Then(@"The page presents a Create Account button")]
        public void ThenThePagePresentsACreateAccountButton()
        {
            _homePage.RegisterButton.Should().NotBeNull();
            _homePage.RegisterButton.Displayed.Should().BeTrue();
        }

        [Then(@"The page presents a Login button")]
        public void ThenThePagePresentsALoginButton()
        {
            _homePage.RegisterButton.Should().NotBeNull();
            _homePage.RegisterButton.Displayed.Should().BeTrue();
        }

        [Then(@"I can save cookies")]
        public void ThenICanSaveCookies()
        {
            _homePage.SaveAllCookies().Should().BeTrue();
        }

        [When(@"I load previously saved cookies")]
        public void WhenILoadPreviouslySavedCookies()
        {
            _homePage.LoadAllCookies().Should().BeTrue();
        }

        [Given(@"I logout"), When(@"I logout")]
        public void GivenILogout()
        {
            _homePage.GoTo();
            _homePage.Logout();
        }

    }
}
