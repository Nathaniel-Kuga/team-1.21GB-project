using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Standups_BDD_Tests.PageObjects;

namespace Team121GB_BDD_Test.PageObjects
{
    public class FriendPageObject : PageObject
    {
        public FriendPageObject(IWebDriver webDriver) : base(webDriver)
        {
            _pageName = "Friend";
        }

        public IWebElement InputEmail => _webDriver.FindElement(By.Id("EmailInput"));
        public IWebElement Picture => _webDriver.FindElement(By.Id("profilePicture"));
        public IWebElement NotFoundError => _webDriver.FindElement(By.Id("notFound"));
        public IWebElement FriendButton => _webDriver.FindElement(By.Id("submitFindFriends"));
    }
}
