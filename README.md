# AutomationExercise - Selenium WebDriver (C#)

A professional Selenium WebDriver automation framework built with C#, demonstrating test implementation across three major .NET test frameworks: **NUnit**, **xUnit**, and **MSTest**.

All test scenarios target [automationexercise.com](https://automationexercise.com/) and are implemented identically across each framework, allowing direct comparison of syntax, structure, and patterns.

<!-- TODO: Add CI/CD badges here -->
<!-- ![Build Status](https://github.com/YOUR_USERNAME/AutomationExercise-Selenium-CSharp/actions/workflows/ci.yml/badge.svg) -->

---

## Technologies Used

| Category | Technology |
|---|---|
| Language | C# / .NET 8.0 |
| Automation | Selenium WebDriver 4.x |
| Test Frameworks | NUnit, xUnit, MSTest |
| Assertions | FluentAssertions |
| Driver Management | WebDriverManager.Net |
| Configuration | Microsoft.Extensions.Configuration |
| CI/CD | GitHub Actions |
| IDE | Visual Studio 2022 / VS Code |

---

## Project Structure

```
AutomationExercise.Selenium/
├── AutomationExercise.Core/            # Shared framework library
│   ├── Config/                         # App settings & configuration models
│   ├── Drivers/                        # WebDriver factory & lifecycle management
│   ├── Helpers/                        # Wait utilities, screenshots, extensions
│   └── Pages/                          # Page Object Models
├── AutomationExercise.NUnit.Tests/     # NUnit test implementations
│   └── Tests/
├── AutomationExercise.XUnit.Tests/     # xUnit test implementations
│   └── Tests/
├── AutomationExercise.MSTest.Tests/    # MSTest test implementations
│   └── Tests/
├── .github/workflows/                  # CI/CD pipeline definitions
├── .gitignore
├── README.md
└── LICENSE
```

---

## Design Patterns

**Page Object Model (POM)** — Each page of the application is represented by a class encapsulating its elements and actions. Test logic stays separate from page implementation, improving maintainability.

**Factory Pattern** — `DriverFactory` handles WebDriver creation for different browsers (Chrome, Firefox, Edge) with configurable options like headless mode, window size, and timeouts.

**Fluent Assertions** — All test validations use FluentAssertions for readable, expressive assertions with clear failure messages.

---

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (or latest LTS)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (Community or higher)
  - Workloads: *.NET desktop development*, *ASP.NET and web development*
- [Google Chrome](https://www.google.com/chrome/), [Firefox](https://www.mozilla.org/firefox/), or [Microsoft Edge](https://www.microsoft.com/edge)
- Git

**Optional:** [VS Code](https://code.visualstudio.com/) with the C# Dev Kit extension for lightweight editing.

---

## Installation & Setup

### Clone the Repository
```bash
git clone https://github.com/YOUR_USERNAME/AutomationExercise-Selenium-CSharp.git
cd AutomationExercise-Selenium-CSharp
```

### Open in Visual Studio
1. Double-click `AutomationExercise.Selenium.sln`
2. Visual Studio will automatically restore NuGet packages
3. Build the solution: **Build → Build Solution** (Ctrl+Shift+B)

### Open in VS Code (Alternative)
1. Open the folder in VS Code
2. Install the **C# Dev Kit** extension if prompted
3. Restore and build via terminal:
```bash
dotnet restore
dotnet build
```

---

## Running Tests

### Visual Studio Test Explorer
1. Open **Test → Test Explorer** (Ctrl+E, T)
2. Click **Run All** to execute all tests across all frameworks
3. Use the search/filter bar to run tests from a specific framework or category

### Command Line (dotnet CLI)

**Run all tests:**
```bash
dotnet test
```

**Run a specific framework:**
```bash
dotnet test --filter "FullyQualifiedName~NUnit"
dotnet test --filter "FullyQualifiedName~XUnit"
dotnet test --filter "FullyQualifiedName~MSTest"
```

**Run a specific test class:**
```bash
dotnet test --filter "FullyQualifiedName~LoginTests"
```

**Run with a different browser** (via environment variable):
```bash
set AppSettings__Browser=Firefox
dotnet test
```

---

## Configuration

Settings are managed through `AutomationExercise.Core/Config/appsettings.json`:

```json
{
  "AppSettings": {
    "BaseUrl": "https://automationexercise.com",
    "Browser": "Chrome",
    "Headless": false,
    "ImplicitWait": 10,
    "ExplicitWait": 30,
    "PageLoadTimeout": 60,
    "WindowWidth": 1920,
    "WindowHeight": 1080
  }
}
```

Environment-specific overrides are available via `appsettings.Development.json` and `appsettings.CI.json`. Settings can also be overridden through environment variables using the pattern `AppSettings__PropertyName`.

---

## Test Scenarios

| Area | Scenarios |
|---|---|
| Authentication | Login (valid/invalid credentials), logout |
| Products | Search, view details, navigate categories, filter |
| Shopping Cart | Add/remove products, update quantities, verify totals |
| Registration | New user registration, form validation, duplicate email handling |
| Checkout | End-to-end purchase flow, order confirmation |
| Contact & Subscription | Contact form submission, newsletter subscription |

Each scenario is implemented identically across NUnit, xUnit, and MSTest to demonstrate framework-specific syntax and patterns.

---

## Framework Comparison

| Feature | NUnit | xUnit | MSTest |
|---|---|---|---|
| Test Attribute | `[Test]` | `[Fact]` | `[TestMethod]` |
| Setup | `[SetUp]` | Constructor | `[TestInitialize]` |
| Teardown | `[TearDown]` | `IDisposable` | `[TestCleanup]` |
| Parameterized | `[TestCase]` | `[Theory] + [InlineData]` | `[DataRow]` |
| Grouping | `[Category]` | `[Trait]` | `[TestCategory]` |

---

## Future Enhancements

- Cross-browser testing matrix (Firefox, Edge)
- Parallel test execution
- Test reporting (ExtentReports / Allure)
- Screenshot capture on failure
- Docker containerization
- API testing integration
- BDD with SpecFlow

---

## Related Repository

- **[AutomationExercise-Playwright-CSharp](https://github.com/YOUR_USERNAME/AutomationExercise-Playwright-CSharp)** — Same test scenarios implemented with Playwright for direct framework comparison.

---

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

## Contact

<!-- TODO: Update with your details -->
**Your Name** — [GitHub](https://github.com/YOUR_USERNAME) | [LinkedIn](https://linkedin.com/in/YOUR_PROFILE)
