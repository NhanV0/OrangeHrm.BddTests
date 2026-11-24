using Microsoft.Playwright;

namespace OrangeHrm.BddTests.Pages;

/// <summary>Shared behaviour for all page objects. Locators live in pages, assertions live in steps.</summary>
public abstract class BasePage
{
    protected readonly IPage Page;

    protected BasePage(IPage page) => Page = page;

    // Page-specific relative path, e.g. "/web/index.php/auth/login"
    protected abstract string RelativePath { get; }

    public async Task OpenAsync() => await Page.GotoAsync(RelativePath);
}
