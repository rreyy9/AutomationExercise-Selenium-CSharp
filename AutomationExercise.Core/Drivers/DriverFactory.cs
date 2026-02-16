using AutomationExercise.Core.Config;
using AutomationExercise.Core.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace AutomationExercise.Core.Drivers;

/// <summary>
/// Factory for creating and configuring WebDriver instances.
/// Supports Chrome, Firefox, and Edge with configurable options
/// including headless mode, window size, and timeouts.
/// 
/// Uses Selenium Manager (built into Selenium 4.6+) for automatic
/// driver binary management — no manual driver downloads required.
/// </summary>
public static class DriverFactory
{
    /// <summary>
    /// Creates a new WebDriver instance using settings from appsettings.json.
    /// </summary>
    /// <returns>A configured IWebDriver instance.</returns>
    public static IWebDriver CreateDriver()
    {
        var settings = ConfigHelper.Settings;
        var browserType = ParseBrowserType(settings.Browser);

        return CreateDriver(browserType, settings);
    }

    /// <summary>
    /// Creates a new WebDriver instance for a specific browser type.
    /// Useful for parameterised cross-browser tests.
    /// </summary>
    /// <param name="browserType">The browser to create a driver for.</param>
    /// <returns>A configured IWebDriver instance.</returns>
    public static IWebDriver CreateDriver(BrowserType browserType)
    {
        return CreateDriver(browserType, ConfigHelper.Settings);
    }

    /// <summary>
    /// Creates a new WebDriver instance with explicit settings.
    /// </summary>
    private static IWebDriver CreateDriver(BrowserType browserType, AppSettings settings)
    {
        IWebDriver driver = browserType switch
        {
            BrowserType.Chrome => CreateChromeDriver(settings),
            BrowserType.Firefox => CreateFirefoxDriver(settings),
            BrowserType.Edge => CreateEdgeDriver(settings),
            _ => throw new ArgumentOutOfRangeException(nameof(browserType), $"Browser type '{browserType}' is not supported.")
        };

        ConfigureDriver(driver, settings);
        return driver;
    }

    private static IWebDriver CreateChromeDriver(AppSettings settings)
    {
        var options = new ChromeOptions();

        if (settings.Headless)
        {
            options.AddArgument("--headless=new");
        }

        options.AddArgument($"--window-size={settings.WindowWidth},{settings.WindowHeight}");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");

        // Suppress unwanted logging in test output
        options.AddArgument("--log-level=3");
        options.AddExcludedArgument("enable-logging");

        return new ChromeDriver(options);
    }

    private static IWebDriver CreateFirefoxDriver(AppSettings settings)
    {
        var options = new FirefoxOptions();

        if (settings.Headless)
        {
            options.AddArgument("--headless");
        }

        options.AddArgument($"--width={settings.WindowWidth}");
        options.AddArgument($"--height={settings.WindowHeight}");

        return new FirefoxDriver(options);
    }

    private static IWebDriver CreateEdgeDriver(AppSettings settings)
    {
        var options = new EdgeOptions();

        if (settings.Headless)
        {
            options.AddArgument("--headless=new");
        }

        options.AddArgument($"--window-size={settings.WindowWidth},{settings.WindowHeight}");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");

        return new EdgeDriver(options);
    }

    /// <summary>
    /// Applies common driver-level configuration: timeouts and window management.
    /// </summary>
    private static void ConfigureDriver(IWebDriver driver, AppSettings settings)
    {
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(settings.ImplicitWait);
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(settings.PageLoadTimeout);
        driver.Manage().Window.Maximize();
    }

    /// <summary>
    /// Parses a browser name string into a BrowserType enum value.
    /// </summary>
    private static BrowserType ParseBrowserType(string browser)
    {
        if (Enum.TryParse<BrowserType>(browser, ignoreCase: true, out var browserType))
        {
            return browserType;
        }

        throw new ArgumentException(
            $"Unsupported browser: '{browser}'. Supported values: {string.Join(", ", Enum.GetNames<BrowserType>())}");
    }
}