namespace AutomationExercise.Core.Config;

/// <summary>
/// Strongly-typed configuration model bound from appsettings.json.
/// Provides type-safe access to all test framework settings.
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Base URL of the application under test.
    /// </summary>
    public string BaseUrl { get; set; } = "https://automationexercise.com";

    /// <summary>
    /// Browser to use for test execution (Chrome, Firefox, Edge).
    /// </summary>
    public string Browser { get; set; } = "Chrome";

    /// <summary>
    /// Whether to run the browser in headless mode.
    /// </summary>
    public bool Headless { get; set; } = false;

    /// <summary>
    /// Implicit wait timeout in seconds.
    /// Applied globally to the driver for element lookups.
    /// </summary>
    public int ImplicitWait { get; set; } = 10;

    /// <summary>
    /// Default explicit wait timeout in seconds.
    /// Used by WaitHelper for conditional waits.
    /// </summary>
    public int ExplicitWait { get; set; } = 30;

    /// <summary>
    /// Page load timeout in seconds.
    /// Maximum time to wait for a page to finish loading.
    /// </summary>
    public int PageLoadTimeout { get; set; } = 60;

    /// <summary>
    /// Browser window width in pixels.
    /// </summary>
    public int WindowWidth { get; set; } = 1920;

    /// <summary>
    /// Browser window height in pixels.
    /// </summary>
    public int WindowHeight { get; set; } = 1080;
}