using Microsoft.Extensions.Configuration;

namespace AutomationExercise.Core.Config;

/// <summary>
/// Provides centralised access to application configuration.
/// Reads from appsettings.json with environment-specific overrides
/// and environment variable support for CI/CD pipelines.
/// </summary>
public static class ConfigHelper
{
    private static readonly Lazy<AppSettings> _settings = new(LoadSettings);

    /// <summary>
    /// Gets the current application settings.
    /// Settings are loaded once and cached for the lifetime of the test run.
    /// </summary>
    public static AppSettings Settings => _settings.Value;

    private static AppSettings LoadSettings()
    {
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(GetConfigBasePath())
            .AddJsonFile("Config/appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile($"Config/appsettings.{environment}.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables(prefix: "AE_")
            .Build();

        var settings = new AppSettings();
        configuration.GetSection("AppSettings").Bind(settings);

        return settings;
    }

    /// <summary>
    /// Resolves the base path for configuration files.
    /// Handles differences between running from IDE vs. dotnet CLI.
    /// </summary>
    private static string GetConfigBasePath()
    {
        // When running tests, the working directory is typically the output directory (bin/Debug/net10.0)
        // We need to find where appsettings.json lives relative to that
        var basePath = AppDomain.CurrentDomain.BaseDirectory;

        // Check if Config/appsettings.json exists in the base directory (copied to output)
        if (File.Exists(Path.Combine(basePath, "Config", "appsettings.json")))
        {
            return basePath;
        }

        // Fallback: walk up from output directory to find project root
        var directory = new DirectoryInfo(basePath);
        while (directory != null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Config", "appsettings.json")))
            {
                return directory.FullName;
            }
            directory = directory.Parent;
        }

        // Last resort: use current directory
        return Directory.GetCurrentDirectory();
    }
}