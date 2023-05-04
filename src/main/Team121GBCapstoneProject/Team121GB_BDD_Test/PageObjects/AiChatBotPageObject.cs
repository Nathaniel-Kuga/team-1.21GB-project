using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Standups_BDD_Tests.PageObjects;

namespace Team121GB_BDD_Test.PageObjects;

public class AiChatBotPageObject : PageObject
{
    public AiChatBotPageObject(IWebDriver webDriver) : base(webDriver)
    {
        _pageName = "ChatGPT page";
    }

    public IWebElement PromptInput => _webDriver.FindElement(By.Id("prompt"));
    public IWebElement SubmitPromptButton => _webDriver.FindElement(By.Id("send"));
    public void InputPrompt(string prompt) => PromptInput.SendKeys(prompt);

    public int CheckIfResponseExists()
    {
        try
        {
            // Wait up to 10 seconds for the element to be present in the DOM
            WebDriverWait wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
            // Use the CSS selector to find the element
            IReadOnlyCollection<IWebElement> responses = wait.Until(d => d.FindElements(By.CssSelector(".card.text-white.bg-primary")));
            responses.Should().NotBeNullOrEmpty();
            return responses.Count();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return -1;
        }
    }

    //public int CheckIfResponseForInappropriatePromptExists()
    //{
    //    try
    //    {
    //        // Wait up to 10 seconds for an alert to be present
    //        WebDriverWait wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
    //        IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());
    //        // Switch to the alert
    //        _webDriver.SwitchTo().Alert();
    //        // Dismiss the alert
    //        alert.Dismiss();
    //        // Wait up to 10 seconds for the element to be present in the DOM
    //        IReadOnlyCollection<IWebElement> responses = wait.Until(d => d.FindElements(By.CssSelector(".card.text-white.bg-danger")));
    //        responses.Count.Should().Be(1);
    //        return responses.Count();
    //    }
    //    catch (Exception e)
    //    {
    //        Console.WriteLine(e);
    //        return -1;
    //    }
    //}
    public int CheckIfResponseForInappropriatePromptExists()
    {
        try
        {
            // Wait up to 10 seconds for the element to be present in the DOM
            WebDriverWait wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));

            wait.Until(driver => driver.SwitchTo().Alert());
            IAlert alert = _webDriver.SwitchTo().Alert();
            // Dismiss the alert
            alert.Dismiss();
            // Use the CSS selector to find the element
            _webDriver.SwitchTo().DefaultContent();
            IReadOnlyCollection<IWebElement> responses = wait.Until(d => d.FindElements(By.CssSelector(".card.text-white.bg-danger")));
            responses.Count.Should().Be(1);
            return responses.Count();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return -1;
        }
    }

}
