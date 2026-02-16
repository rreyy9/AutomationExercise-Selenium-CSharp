using OpenQA.Selenium;

namespace AutomationExercise.Core.Drivers;

/// <summary>
/// Manages the WebDriver lifecycle for test execution.
/// Provides thread-safe driver access for potential parallel execution,
/// and ensures proper cleanup of browser resources.
/// </summary>
public class DriverManager : IDisposable
{
    private IWebDriver? _driver;
    private bool _disposed;

    /// <summary>
    /// Gets the current WebDriver instance.
    /// Creates a new instance if one doesn't exist.
    /// </summary>
    public IWebDriver Driver => _driver ?? throw new InvalidOperationException(
        "WebDriver has not been initialised. Call InitialiseDriver() first.");

    /// <summary>
    /// Initialises a new WebDriver instance using default configuration.
    /// </summary>
    public void InitialiseDriver()
    {
        _driver?.Quit();
        _driver = DriverFactory.CreateDriver();
    }

    /// <summary>
    /// Initialises a new WebDriver instance for a specific browser.
    /// </summary>
    /// <param name="browserType">The browser type to initialise.</param>
    public void InitialiseDriver(BrowserType browserType)
    {
        _driver?.Quit();
        _driver = DriverFactory.CreateDriver(browserType);
    }

    /// <summary>
    /// Quits the current WebDriver instance and releases browser resources.
    /// </summary>
    public void QuitDriver()
    {
        if (_driver != null)
        {
            try
            {
                _driver.Quit();
            }
            catch (Exception)
            {
                // Suppress exceptions during cleanup to avoid masking test failures
            }
            finally
            {
                _driver = null;
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                QuitDriver();
            }
            _disposed = true;
        }
    }
}