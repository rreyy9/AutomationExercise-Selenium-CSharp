using OpenQA.Selenium;

namespace AutomationExercise.Core.Helpers;

/// <summary>
/// Utility for capturing browser screenshots during test execution.
/// Useful for debugging failed tests and generating evidence of test runs.
/// </summary>
public static class ScreenshotHelper
{
    private const string DefaultScreenshotDirectory = "Screenshots";

    /// <summary>
    /// Captures a screenshot and saves it to the specified directory.
    /// </summary>
    /// <param name="driver">The WebDriver instance to capture from.</param>
    /// <param name="screenshotName">A descriptive name for the screenshot file.</param>
    /// <param name="directory">Optional directory path. Defaults to "Screenshots" in the output folder.</param>
    /// <returns>The full file path of the saved screenshot.</returns>
    public static string CaptureScreenshot(IWebDriver driver, string screenshotName, string? directory = null)
    {
        var screenshotDir = directory ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultScreenshotDirectory);
        Directory.CreateDirectory(screenshotDir);

        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var sanitisedName = SanitiseFileName(screenshotName);
        var fileName = $"{sanitisedName}_{timestamp}.png";
        var filePath = Path.Combine(screenshotDir, fileName);

        var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
        screenshot.SaveAsFile(filePath);

        return filePath;
    }

    /// <summary>
    /// Captures a screenshot with a name derived from the test context.
    /// Intended to be called from test teardown on failure.
    /// </summary>
    /// <param name="driver">The WebDriver instance to capture from.</param>
    /// <param name="testClassName">The name of the test class.</param>
    /// <param name="testMethodName">The name of the test method.</param>
    /// <returns>The full file path of the saved screenshot.</returns>
    public static string CaptureScreenshotOnFailure(IWebDriver driver, string testClassName, string testMethodName)
    {
        var screenshotName = $"FAIL_{testClassName}_{testMethodName}";
        return CaptureScreenshot(driver, screenshotName);
    }

    /// <summary>
    /// Removes invalid file name characters from the screenshot name.
    /// </summary>
    private static string SanitiseFileName(string name)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        return string.Join("_", name.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
    }
}