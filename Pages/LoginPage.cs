using Microsoft.Playwright;

namespace OrangeHrm.BddTests.Pages;

public sealed class LoginPage : BasePage
{
    public LoginPage(IPage page) : base(page) { }

    protected override string RelativePath => "/web/index.php/auth/login";

    // ---- Locators ----
    public ILocator UsernameInput => Page.GetByPlaceholder("Username"); 
    public ILocator PasswordInput => Page.GetByPlaceholder("Password"); 
    public ILocator LoginButton => Page.GetByRole(AriaRole.Button, new() { Name = "Login" });
    public ILocator ErrorAlert => Page.Locator(".oxd-alert-content-text");

    // ---- Actions ----
   public async Task EnterUsernameAsync(string username) => await UsernameInput.FillAsync(username);
   public async Task EnterPasswordAsync(string password) => await PasswordInput.FillAsync(password);
   public async Task ClickLoginButtonAsync() => await LoginButton.ClickAsync();
   public async Task LoginAsync(string username, String password)
   {
        await EnterUsernameAsync(username);
        await EnterPasswordAsync(password);
        await ClickLoginButtonAsync();
   }
}
