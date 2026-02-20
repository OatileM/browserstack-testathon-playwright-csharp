using Microsoft.Playwright;

namespace Testathon.Tests.Pages;

public abstract class BasePage
{
    protected readonly IPage Page;

    protected BasePage(IPage page)
    {
        Page = page;
    }

    protected async Task WaitVisible(string locator)
    {
        await Page.Locator(locator).WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 30000 });
    }

    protected async Task Click(string locator)
    {
        await Page.Locator(locator).ClickAsync();
    }

    protected async Task Fill(string locator, string value)
    {
        await Page.Locator(locator).FillAsync(value);
    }

    protected async Task<string> GetText(string locator)
    {
        return await Page.Locator(locator).TextContentAsync() ?? string.Empty;
    }
}
