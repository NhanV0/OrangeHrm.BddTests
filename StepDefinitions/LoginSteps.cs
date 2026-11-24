using System.Text.RegularExpressions;
using Microsoft.Playwright;
using OrangeHrm.BddTests.Pages;
using OrangeHrm.BddTests.Support;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace OrangeHrm.BddTests.StepDefinitions;

[Binding]
public sealed class LoginSteps
{
    private readonly IPage _page;
    private readonly LoginPage _loginPage;
    private readonly DashboardPage _dashboardPage;

    public LoginSteps(PlaywrightDriver driver)
    {
        _page = driver.Page;
        _loginPage = new LoginPage(driver.Page);
        _dashboardPage = new DashboardPage(driver.Page);
    }
    
    [Given(@"I am on the OrangeHRM login page")]
    public async Task GivenIAmOnTheOrangeHRMLoginPage()
    {
        await _loginPage.OpenAsync();
        await Expect(_loginPage.LoginButton).ToBeVisibleAsync();
    }

    [When(@"I log in with valid admin credentials")]
    public async Task WhenILogInWithValidAdminCredentials()
    {
        var admin = TestSettings.Current.AdminUser;
        await _loginPage.LoginAsync(admin.Username, admin.Password);
    }

    [Then(@"I should be redirected to the dashboard page")]
    public async Task ThenIShouldBeRedirectedToTheDashboardPage()
    {
        await Expect(_dashboardPage.Header.First).ToBeVisibleAsync();
        await Expect(_page).ToHaveURLAsync(new Regex(@"/dashboard/index$"));
    }

    [Then(@"The dashboard page header should be ""(.*)""")]
    public async Task ThenTheDashboardPageHeaderShouldBe(string expectedHeader)
    {
        await Expect(_dashboardPage.Header.First).ToHaveTextAsync(expectedHeader);
    }

    [Then(@"The user menu should be visible")]
    public async Task ThenTheUserMenuShouldBeVisible()
    {
        await Expect(_dashboardPage.UserDropdown).ToBeVisibleAsync();
    }

    [When(@"I log in with username ""(.*)"" and password ""(.*)""")]
    public async Task WhenILogInWithUsernameAndPassword(string username, string password)
    {
        await _loginPage.LoginAsync(username, password);
    }

    [Then(@"I should see an error message ""(.*)""")]
    public async Task ThenIShouldSeeAnErrorMessage(string expectedErrorMessage)
    {
        await Expect(_loginPage.ErrorAlert).ToBeVisibleAsync();
        await Expect(_loginPage.ErrorAlert).ToHaveTextAsync(expectedErrorMessage);
    }

    [Then(@"I should remain on the login page")]
    public async Task ThenIShouldRemainOnTheLoginPage()
    {
        await Expect(_loginPage.LoginButton).ToBeVisibleAsync();
        await Expect(_page).ToHaveURLAsync(new Regex(@"/auth/login"));
    }
}
