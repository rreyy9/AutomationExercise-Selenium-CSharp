using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace AutomationExercise.Core.Helpers;

/// <summary>
/// Extension methods for IWebDriver and IWebElement to simplify
/// common operations across page objects and tests.
/// </summary>
public static class ExtensionMethods
{
    // ─── IWebDriver Extensions ──────────────────────────────────────────

    /// <summary>
    /// Executes a JavaScript script and returns the result.
    /// </summary>
    public static object? ExecuteScript(this IWebDriver driver, string script, params object[] args)
    {
        return ((IJavaScriptExecutor)driver).ExecuteScript(script, args);
    }

    /// <summary>
    /// Waits for the page to be fully loaded using document.readyState.
    /// </summary>
    public static void WaitForPageLoad(this IWebDriver driver, int timeoutSeconds = 30)
    {
        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
        wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState")?.ToString() == "complete");
    }

    /// <summary>
    /// Switches to a new browser window/tab that was opened after the action.
    /// </summary>
    public static void SwitchToNewWindow(this IWebDriver driver, string originalWindowHandle)
    {
        var newHandle = driver.WindowHandles.First(h => h != originalWindowHandle);
        driver.SwitchTo().Window(newHandle);
    }

    /// <summary>
    /// Accepts a JavaScript alert if one is present.
    /// Returns true if an alert was found and accepted, false otherwise.
    /// </summary>
    public static bool TryAcceptAlert(this IWebDriver driver)
    {
        try
        {
            driver.SwitchTo().Alert().Accept();
            return true;
        }
        catch (NoAlertPresentException)
        {
            return false;
        }
    }

    // ─── IWebElement Extensions ─────────────────────────────────────────

    /// <summary>
    /// Clears the field and types text, simulating realistic user input.
    /// </summary>
    public static void ClearAndType(this IWebElement element, string text)
    {
        element.Clear();
        element.SendKeys(text);
    }

    /// <summary>
    /// Checks if the element has a specific CSS class.
    /// </summary>
    public static bool HasClass(this IWebElement element, string className)
    {
        var classes = element.GetAttribute("class") ?? string.Empty;
        return classes.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                      .Contains(className, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Gets the trimmed, visible text of an element.
    /// </summary>
    public static string GetTrimmedText(this IWebElement element)
    {
        return element.Text.Trim();
    }

    /// <summary>
    /// Scrolls the element into view using JavaScript.
    /// </summary>
    public static void ScrollIntoView(this IWebElement element, IWebDriver driver)
    {
        ((IJavaScriptExecutor)driver).ExecuteScript(
            "arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", element);
    }

    /// <summary>
    /// Clicks an element using JavaScript.
    /// Useful when standard Click() is intercepted by overlays.
    /// </summary>
    public static void ClickViaJavaScript(this IWebElement element, IWebDriver driver)
    {
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
    }

    /// <summary>
    /// Hovers over an element using Selenium Actions.
    /// </summary>
    public static void Hover(this IWebElement element, IWebDriver driver)
    {
        new Actions(driver).MoveToElement(element).Perform();
    }

    /// <summary>
    /// Highlights an element briefly for visual debugging.
    /// Sets a red border that can help identify elements during development.
    /// </summary>
    public static void Highlight(this IWebElement element, IWebDriver driver)
    {
        ((IJavaScriptExecutor)driver).ExecuteScript(
            "arguments[0].style.border='3px solid red'", element);
    }
}