using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Standups_BDD_Tests.Shared;
using System.Collections.ObjectModel;

namespace Standups_BDD_Tests.PageObjects
{
    public class HomePageObject : PageObject
    {
        public HomePageObject(IWebDriver webDriver) : base(webDriver)
        {
            // using a named page (in Common.cs)
            _pageName = "Home";
        }

        public IWebElement RegisterButton => _webDriver.FindElement(By.Id("registerBtn"));
        public IWebElement NavBarHelloLink => _webDriver.FindElement(By.Id("HelloLink"));
        public IWebElement Top100GamesButton => _webDriver.FindElement(By.Id("Top100GamesButton"));
        public IWebElement NavbarBarDropDownLoggedIn => _webDriver.FindElement(By.Id("navbarDropdownMenuLink"));
        public IWebElement GamesListButtonLoggedIn => _webDriver.FindElement(By.Id("gamesListButton"));
        public string NavbarWelcomeText()
        {
            return NavBarHelloLink.Text;
        }

        public void Logout()
        {
            IWebElement profilePictureBtn = _webDriver.FindElement(By.Id("navbarDropdownMenuLink"));
            profilePictureBtn.Click();
            IWebElement navbarLogoutButton = _webDriver.FindElement(By.Id("logoutBtn"));
            navbarLogoutButton.Click();
        }
    }
}