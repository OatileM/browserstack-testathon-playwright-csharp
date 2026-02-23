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
        var bsDevice = Environment.GetEnvironmentVariable("BROWSERSTACK_DEVICE") ?? "chrome-win11";

        PW = await Playwright.CreateAsync();

        if (!string.IsNullOrWhiteSpace(bsUsername) && !string.IsNullOrWhiteSpace(bsAccessKey))
        {
            Console.WriteLine($"Connecting to BrowserStack Automate with device: {bsDevice}");
            
            var caps = GetBrowserStackCapabilities(bsDevice, bsUsername, bsAccessKey);
            var wsEndpoint = $"wss://cdp.browserstack.com/playwright?caps={System.Text.Json.JsonSerializer.Serialize(caps)}";
            
            Browser = await PW.Chromium.ConnectOverCDPAsync(wsEndpoint);
            Context = Browser.Contexts[0];
            Page = Context.Pages[0];
        }
        else
        {
            Console.WriteLine("Running locally - BrowserStack credentials not set");
            Browser = await PW.Chromium.LaunchAsync(new() { Headless = false });
            Context = await Browser.NewContextAsync();
            Page = await Context.NewPageAsync();
        }
    }

    private Dictionary<string, object> GetBrowserStackCapabilities(string device, string username, string accessKey)
    {
        var caps = new Dictionary<string, object>
        {
            { "browserstack.username", username },
            { "browserstack.accessKey", accessKey },
            { "name", TestContext.CurrentContext.Test.Name },
            { "build", "testathon-playwright-csharp" },
            { "project", "BrowserStack Testathon" }
        };

        switch (device)
        {
            case "chrome-win11":
                caps["browser"] = "chrome";
                caps["browser_version"] = "latest";
                caps["os"] = "Windows";
                caps["os_version"] = "11";
                break;
            case "chrome-mac":
                caps["browser"] = "chrome";
                caps["browser_version"] = "latest";
                caps["os"] = "osx";
                caps["os_version"] = "Sonoma";
                break;
            case "edge-win11":
                caps["browser"] = "edge";
                caps["browser_version"] = "latest";
                caps["os"] = "Windows";
                caps["os_version"] = "11";
                break;
            default:
                caps["browser"] = "chrome";
                caps["browser_version"] = "latest";
                caps["os"] = "Windows";
                caps["os_version"] = "11";
                break;
        }

        return caps;
    }

    [TearDown]
    public async Task TearDown()
    {
        await Page.CloseAsync();
        await Browser.CloseAsync();
        PW.Dispose();
    }
}