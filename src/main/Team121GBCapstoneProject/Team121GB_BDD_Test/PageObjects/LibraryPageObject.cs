using OpenQA.Selenium;
using Standups_BDD_Tests.PageObjects;

namespace Team121GB_BDD_Test.PageObjects;

public class LibraryPageObject : PageObject
{
    public LibraryPageObject(IWebDriver webDriver) : base(webDriver)
    {
        _pageName = "Games Lists";
    }

    public IWebElement CurrentlyPlayingTable => _webDriver.FindElement(By.Id("Currently Playing Table"));
    public IWebElement CompletedTable => _webDriver.FindElement(By.Id("Completed Table"));
    public IWebElement WantToPlayTable => _webDriver.FindElement(By.Id("Want to Play Table"));

}