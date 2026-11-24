using Microsoft.Playwright;

namespace OrangeHrm.BddTests.Pages;

public sealed class DashboardPage : BasePage
{
    public DashboardPage(IPage page) : base(page) { }

    protected override string RelativePath => "/web/index.php/dashboard/index";

    public ILocator Header => Page.Locator(".oxd-topbar-header-breadcrumb h6");
    public ILocator UserDropdown => Page.Locator(".oxd-userdropdown-tab");
}
