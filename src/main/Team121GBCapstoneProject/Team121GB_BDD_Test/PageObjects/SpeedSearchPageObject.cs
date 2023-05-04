using OpenQA.Selenium;
using Standups_BDD_Tests.PageObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team121GB_BDD_Test.PageObjects
{
    public class SpeedSearchPageObject : PageObject
    {
        public SpeedSearchPageObject(IWebDriver webDriver) : base(webDriver)
        {
            _pageName = "SpeedSearch";
        }

        public IWebElement GamesInput => _webDriver.FindElement(By.Id("GameEntry")); 

        public IWebElement GamesSubmitButton => _webDriver.FindElement(By.Id("games-submit"));

        public void EnterGames(string Games)
        {
            GamesInput.Clear();
            GamesInput.SendKeys(Games);
        }

        public void SubmitGames()
        {
            GamesSubmitButton.Click();
        }

        public bool HasGameErrors()
        {
            ReadOnlyCollection<IWebElement> elements = _webDriver.FindElements(By.CssSelector(".validation-summary-errors"));
            return elements.Count() > 0;
        }
    }
}
