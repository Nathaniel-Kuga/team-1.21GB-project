using OpenQA.Selenium;

namespace Team121GB_BDD_Test.PageObjects;

public class Top100GamesPageObject : ProfilePageObject
{
    public Top100GamesPageObject(IWebDriver webDriver) : base(webDriver)
    {
        _pageName = "Top 100 Games Page";
    }
}