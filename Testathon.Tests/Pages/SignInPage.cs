using Microsoft.Playwright;

namespace Testathon.Tests.Pages;

public class SignInPage : BasePage
{
    public const string Url = "https://testathon.live/signin";

    private const string UsernameDropdown = "#username";
    private const string PasswordDropdown = "#password";
    private const string LoginButton = "#login-btn";

    public SignInPage(IPage page) : base(page) { }

    public async Task Navigate()
    {
        await Page.GotoAsync(Url);
    }

    public async Task Login(string username = "demouser", string password = "testingisfun99")
    {
        await Page.Locator(UsernameDropdown).ClickAsync();
        await Page.Locator($"#react-select-2-option-0-0").ClickAsync();
        
        await Page.Locator(PasswordDropdown).ClickAsync();
        await Page.Locator($"#react-select-3-option-0-0").ClickAsync();
        
        await Page.Locator(LoginButton).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
}
