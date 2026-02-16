namespace AutomationExercise.Core.Drivers;

/// <summary>
/// Supported browser types for test execution.
/// Used by DriverFactory to determine which WebDriver to instantiate.
/// </summary>
public enum BrowserType
{
    Chrome,
    Firefox,
    Edge
}