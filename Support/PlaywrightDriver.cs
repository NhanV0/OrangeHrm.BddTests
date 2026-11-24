using Microsoft.Playwright;

namespace OrangeHrm.BddTests.Support;

/// <summary>
/// Holds the Playwright context and page of one scenario. Reqnroll creates it per scenario
/// and injects it into the hooks and step definitions.
/// </summary>
public sealed class PlaywrightDriver
{
    public IBrowserContext Context { get; private set; } = default!;
    public IPage Page { get; private set; } = default!;

    public async Task StartAsync(IBrowser browser, TestSettings settings)
    {
        // A fresh context per scenario keeps cookies and sessions isolated.
        Context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1440, Height = 900 },
            BaseURL = settings.BaseUrl
        });
        Context.SetDefaultTimeout(settings.DefaultTimeoutMs);
        Page = await Context.NewPageAsync();
    }

    public async Task StopAsync()
    {
        if (Context is not null)
        {
            await Context.CloseAsync();
        }
    }
}
