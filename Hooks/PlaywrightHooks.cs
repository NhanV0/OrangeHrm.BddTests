using Allure.Net.Commons;
using Microsoft.Playwright;
using OrangeHrm.BddTests.Support;
using Reqnroll;

namespace OrangeHrm.BddTests.Hooks;

[Binding]
public sealed class PlaywrightHooks
{
    // The browser is launched once per test run and shared; each scenario gets its own context.
    private static IPlaywright? _playwright;
    private static IBrowser? _browser;

    private readonly PlaywrightDriver _driver;
    private readonly ScenarioContext _scenarioContext;

    public PlaywrightHooks(PlaywrightDriver driver, ScenarioContext scenarioContext)
    {
        _driver = driver;
        _scenarioContext = scenarioContext;
    }

    [BeforeTestRun]
    public static async Task LaunchBrowserAsync()
    {
        var settings = TestSettings.Current;
        _playwright = await Playwright.CreateAsync();

        var type = settings.Browser.ToLowerInvariant() switch
        {
            "firefox" => _playwright.Firefox,
            "webkit" => _playwright.Webkit,
            _ => _playwright.Chromium
        };

        _browser = await type.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = settings.Headless,
            SlowMo = settings.SlowMoMs
        });
    }

    [BeforeScenario]
    public async Task StartScenarioAsync()
    {
        await _driver.StartAsync(_browser!, TestSettings.Current);
    }

    [AfterScenario]
    public async Task StopScenarioAsync()
    {
        // Attach a screenshot to the Allure report when a scenario fails.
        if (_scenarioContext.TestError is not null && TestSettings.Current.ScreenshotOnFailure)
        {
            var png = await _driver.Page.ScreenshotAsync(new PageScreenshotOptions { FullPage = true });
            AllureApi.AddAttachment("Screenshot on failure", "image/png", png, ".png");
        }

        await _driver.StopAsync();
    }

    [AfterTestRun]
    public static async Task CloseBrowserAsync()
    {
        if (_browser is not null) await _browser.CloseAsync();
        _playwright?.Dispose();
    }
}
