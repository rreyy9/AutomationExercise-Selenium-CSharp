using OpenQA.Selenium;
using AutomationExercise.Core.Config;
using AutomationExercise.Core.Helpers;

namespace AutomationExercise.Core.Pages;

/// <summary>
/// Base class for all Page Object Models.
/// Provides shared functionality including driver access, navigation,
/// common element interactions, and wait utilities.
/// 
/// All page objects should inherit from this class to ensure
/// consistent behaviour and reduce code duplication.
/// </summary>
public abstract class BasePage
{
    protected readonly IWebDriver Driver;
    protected readonly WaitHelper Wait;
    protected readonly AppSettings Settings;

    /// <summary>
    /// The relative URL path for this page (e.g., "/login" or "/products").
    /// Override in derived page classes.
    /// </summary>
    protected abstract string PagePath { get; }

    protected BasePage(IWebDriver driver)
    {
        Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        Wait = new WaitHelper(driver);
        Settings = ConfigHelper.Settings;
    }

    /// <summary>
    /// Gets the full URL for this page by combining BaseUrl with PagePath.
    /// </summary>
    public string PageUrl => $"{Settings.BaseUrl.TrimEnd('/')}/{PagePath.TrimStart('/')}";

    /// <summary>
    /// Navigates to this page using its configured URL.
    /// </summary>
    /// <returns>The current page instance for fluent chaining.</returns>
    public virtual BasePage NavigateTo()
    {
        Driver.Navigate().GoToUrl(PageUrl);
        return this;
    }

    /// <summary>
    /// Gets the current page title from the browser.
    /// </summary>
    public string GetPageTitle() => Driver.Title;

    /// <summary>
    /// Gets the current URL from the browser.
    /// </summary>
    public string GetCurrentUrl() => Driver.Url;

    /// <summary>
    /// Checks whether the current URL contains the expected page path.
    /// Useful for verifying navigation without exact URL matching.
    /// </summary>
    public bool IsOnPage() => Driver.Url.Contains(PagePath, StringComparison.OrdinalIgnoreCase);

    // ─── Common Element Interactions ────────────────────────────────────

    /// <summary>
    /// Finds a single element, waiting for it to be visible first.
    /// </summary>
    protected IWebElement FindElement(By locator)
    {
        return Wait.UntilElementIsVisible(locator);
    }

    /// <summary>
    /// Finds all elements matching the locator.
    /// </summary>
    protected IReadOnlyCollection<IWebElement> FindElements(By locator)
    {
        return Driver.FindElements(locator);
    }

    /// <summary>
    /// Clicks an element after waiting for it to be clickable.
    /// </summary>
    protected void Click(By locator)
    {
        Wait.UntilElementIsClickable(locator).Click();
    }

    /// <summary>
    /// Clears an input field and types the specified text.
    /// </summary>
    protected void Type(By locator, string text)
    {
        var element = Wait.UntilElementIsVisible(locator);
        element.Clear();
        element.SendKeys(text);
    }

    /// <summary>
    /// Gets the visible text content of an element.
    /// </summary>
    protected string GetText(By locator)
    {
        return FindElement(locator).Text;
    }

    /// <summary>
    /// Gets the value of a specified attribute on an element.
    /// </summary>
    protected string? GetAttribute(By locator, string attribute)
    {
        return FindElement(locator).GetAttribute(attribute);
    }

    /// <summary>
    /// Checks whether an element is currently displayed on the page.
    /// Returns false if the element doesn't exist rather than throwing.
    /// </summary>
    protected bool IsElementDisplayed(By locator)
    {
        try
        {
            return Driver.FindElement(locator).Displayed;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
        catch (StaleElementReferenceException)
        {
            return false;
        }
    }

    /// <summary>
    /// Scrolls the page to bring the specified element into view.
    /// </summary>
    protected void ScrollToElement(By locator)
    {
        var element = FindElement(locator);
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", element);
    }

    /// <summary>
    /// Scrolls to the bottom of the page.
    /// </summary>
    protected void ScrollToBottom()
    {
        ((IJavaScriptExecutor)Driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
    }

    /// <summary>
    /// Scrolls to the top of the page.
    /// </summary>
    protected void ScrollToTop()
    {
        ((IJavaScriptExecutor)Driver).ExecuteScript("window.scrollTo(0, 0);");
    }
}