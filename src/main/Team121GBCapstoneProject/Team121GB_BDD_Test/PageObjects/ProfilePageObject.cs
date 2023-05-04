using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Standups_BDD_Tests.PageObjects;

namespace Team121GB_BDD_Test.PageObjects
{
    public class ProfilePageObject : PageObject
    {
        public IWebElement firstNameBox => _webDriver.FindElement(By.Id("FirstName"));
        public IWebElement lastNameBox => _webDriver.FindElement(By.Id("LastName"));
        public IWebElement NavBarHelloLink => _webDriver.FindElement(By.Id("HelloLink"));
        public IWebElement firstNameError;
        public IWebElement lastNameError;
        public IWebElement profilePictureButton => _webDriver.FindElement(By.Id("Input_ProfilePicture"));
        public IWebElement profileUpload => _webDriver.FindElement(By.Id("profilePicture"));
        public IWebElement generateDalleImageButton => _webDriver.FindElement(By.Id("generateImagePageRedirectButton")); 
        public IWebElement findFriendsBtn => _webDriver.FindElement(By.Id("findFriendsBtn"));

        public string NavbarWelcomeText()
        {
            return NavBarHelloLink.Text;
        }

        public ProfilePageObject(IWebDriver webDriver) : base(webDriver)
        {
            _pageName = "Profile";
        }
        public void Logout()
        {
            IWebElement profilePictureBtn = _webDriver.FindElement(By.Id("navbarDropdownMenuLink"));
            profilePictureBtn.Click();
            IWebElement navbarLogoutButton = _webDriver.FindElement(By.Id("logoutBtn"));
            navbarLogoutButton.Click();
        }

        public void Submit()
        {
            IWebElement profileSubmit = _webDriver.FindElement(By.Id("update-profile-bio-button"));
            profileSubmit.Click();
        }

        public void FindErrorText()
        {
            firstNameError = _webDriver.FindElement(By.Id("FirstName-error"));
            lastNameError = _webDriver.FindElement(By.Id("LastName-error"));
        }

        public void UploadPhoto()
        {
            profilePictureButton.SendKeys("C:\\Users\\Cyber\\source\\repos\\Team-1.21GB-Project\\src\\main\\Team121GBCapstoneProject\\Team121GBCapstoneProject\\wwwroot\\images\\logo15Percent.png");

        }
    }
}
