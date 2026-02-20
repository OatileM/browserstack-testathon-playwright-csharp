using Microsoft.Playwright;
using NUnit.Framework;

namespace Testathon.Tests.Framework;

public abstract class TestBase
{
    protected IPlaywright PW = null!;
    protected IBrowser Browser = null!;
    protected IBrowserContext Context = null!;
    protected IPage Page = null!;

    [SetUp]
    public async Task SetUp()
    {
        var bsUsername = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
        var bsAccessKey = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");

        PW = await Playwright.CreateAsync();

        if (!string.IsNullOrEmpty(bsUsername) && !string.IsNullOrEmpty(bsAccessKey))
        {
            var caps = new Dictionary<string, object>
            {
                { "browser", "chrome" },
                { "browser_version", "latest" },
                { "os", "Windows" },
                { "os_version", "11" },
                { "name", TestContext.CurrentContext.Test.Name },
                { "build", "testathon-playwright-csharp" },
                { "browserstack.username", bsUsername },
                { "browserstack.accessKey", bsAccessKey }
            };
            var cdpUrl = $"wss://cdp.browserstack.com/playwright?caps={System.Text.Json.JsonSerializer.Serialize(caps)}";
            Browser = await PW.Chromium.ConnectOverCDPAsync(cdpUrl);
            Context = Browser.Contexts[0];
            Page = Context.Pages[0];
        }
        else
        {
            Browser = await PW.Chromium.LaunchAsync(new() { Headless = false });
            Context = await Browser.NewContextAsync();
            Page = await Context.NewPageAsync();
        }
    }

    [TearDown]
    public async Task TearDown()
    {
        await Page.CloseAsync();
        await Browser.CloseAsync();
        PW.Dispose();
    }
}
