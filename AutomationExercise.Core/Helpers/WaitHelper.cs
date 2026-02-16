using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AutomationExercise.Core.Config;

namespace AutomationExercise.Core.Helpers;

/// <summary>
/// Provides explicit wait utilities for common WebDriver conditions.
/// Wraps Selenium's WebDriverWait with convenient methods for
/// waiting on element visibility, clickability, and other states.
/// 
/// Preferred over implicit waits for more predictable test behaviour
/// and better error messages when waits time out.
/// </summary>
public class WaitHelper
{
    private readonly IWebDriver _driver;
    private readonly TimeSpan _defaultTimeout;
    private readonly TimeSpan _pollingInterval;

    public WaitHelper(IWebDriver driver)
        : this(driver, TimeSpan.FromSeconds(ConfigHelper.Settings.ExplicitWait))
    {
    }

    public WaitHelper(IWebDriver driver, TimeSpan timeout)
    {
        _driver = driver ?? throw new ArgumentNullException(nameof(driver));
        _defaultTimeout = timeout;
        _pollingInterval = TimeSpan.FromMilliseconds(500);
    }

    // ─── Element Visibility ─────────────────────────────────────────────

    /// <summary>
    /// Waits until an element is present in the DOM and visible.
    /// </summary>
    public IWebElement UntilElementIsVisible(By locator, TimeSpan? timeout = null)
    {
        var wait = CreateWait(timeout);
        return wait.Until(driver =>
        {
            var element = driver.FindElement(locator);
            return element.Displayed ? element : null!;
        });
    }

    /// <summary>
    /// Waits until all elements matching the locator are visible.
    /// </summary>
    public IReadOnlyCollection<IWebElement> UntilElementsAreVisible(By locator, TimeSpan? timeout = null)
    {
        var wait = CreateWait(timeout);
        return wait.Until(driver =>
        {
            var elements = driver.FindElements(locator);
            return elements.Count > 0 && elements.All(e => e.Displayed)
                ? elements
                : null!;
        });
    }

    /// <summary>
    /// Waits until an element is no longer visible or no longer in the DOM.
    /// </summary>
    public bool UntilElementIsNotVisible(By locator, TimeSpan? timeout = null)
    {
        var wait = CreateWait(timeout);
        return wait.Until(driver =>
        {
            try
            {
                var element = driver.FindElement(locator);
                return !element.Displayed;
            }
            catch (NoSuchElementException)
            {
                return true;
            }
            catch (StaleElementReferenceException)
            {
                return true;
            }
        });
    }

    // ─── Element Clickability ───────────────────────────────────────────

    /// <summary>
    /// Waits until an element is visible and enabled (clickable).
    /// </summary>
    public IWebElement UntilElementIsClickable(By locator, TimeSpan? timeout = null)
    {
        var wait = CreateWait(timeout);
        return wait.Until(driver =>
        {
            var element = driver.FindElement(locator);
            return element.Displayed && element.Enabled ? element : null!;
        });
    }

    // ─── Element Existence ──────────────────────────────────────────────

    /// <summary>
    /// Waits until an element is present in the DOM (may not be visible).
    /// </summary>
    public IWebElement UntilElementExists(By locator, TimeSpan? timeout = null)
    {
        var wait = CreateWait(timeout);
        return wait.Until(driver => driver.FindElement(locator));
    }

    // ─── Text Conditions ────────────────────────────────────────────────

    /// <summary>
    /// Waits until the specified element contains the expected text.
    /// </summary>
    public bool UntilTextIsPresent(By locator, string text, TimeSpan? timeout = null)
    {
        var wait = CreateWait(timeout);
        return wait.Until(driver =>
        {
            try
            {
                var element = driver.FindElement(locator);
                return element.Text.Contains(text, StringComparison.OrdinalIgnoreCase);
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        });
    }

    // ─── URL & Page Conditions ──────────────────────────────────────────

    /// <summary>
    /// Waits until the browser URL contains the specified text.
    /// </summary>
    public bool UntilUrlContains(string urlPart, TimeSpan? timeout = null)
    {
        var wait = CreateWait(timeout);
        return wait.Until(driver => driver.Url.Contains(urlPart, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Waits until the page title contains the specified text.
    /// </summary>
    public bool UntilTitleContains(string titlePart, TimeSpan? timeout = null)
    {
        var wait = CreateWait(timeout);
        return wait.Until(driver => driver.Title.Contains(titlePart, StringComparison.OrdinalIgnoreCase));
    }

    // ─── Custom Conditions ──────────────────────────────────────────────

    /// <summary>
    /// Waits until a custom condition returns a truthy value.
    /// Provides flexibility for conditions not covered by built-in methods.
    /// </summary>
    public TResult UntilCondition<TResult>(Func<IWebDriver, TResult> condition, TimeSpan? timeout = null)
    {
        var wait = CreateWait(timeout);
        return wait.Until(condition);
    }

    // ─── Frame Handling ─────────────────────────────────────────────────

    /// <summary>
    /// Waits until a frame is available and switches to it.
    /// </summary>
    public IWebDriver UntilFrameIsAvailable(By locator, TimeSpan? timeout = null)
    {
        var wait = CreateWait(timeout);
        return wait.Until(driver =>
        {
            try
            {
                var frameElement = driver.FindElement(locator);
                return driver.SwitchTo().Frame(frameElement);
            }
            catch (NoSuchFrameException)
            {
                return null!;
            }
        });
    }

    // ─── Alert Handling ─────────────────────────────────────────────────

    /// <summary>
    /// Waits until a JavaScript alert is present.
    /// </summary>
    public IAlert UntilAlertIsPresent(TimeSpan? timeout = null)
    {
        var wait = CreateWait(timeout);
        return wait.Until(driver =>
        {
            try
            {
                return driver.SwitchTo().Alert();
            }
            catch (NoAlertPresentException)
            {
                return null!;
            }
        });
    }

    // ─── Private Helpers ────────────────────────────────────────────────

    private WebDriverWait CreateWait(TimeSpan? timeout = null)
    {
        var wait = new WebDriverWait(_driver, timeout ?? _defaultTimeout)
        {
            PollingInterval = _pollingInterval
        };

        wait.IgnoreExceptionTypes(
            typeof(NoSuchElementException),
            typeof(StaleElementReferenceException));

        return wait;
    }
}