using Microsoft.Playwright;

namespace Testathon.Tests.Pages;

public class OrdersPage : BasePage
{
    public const string Url = "https://testathon.live/orders";

    private const string OrdersListing = ".orders-listing";
    private const string OrderCard = ".order";
    private const string OrderTotal = ".value";
    private const string OrderShipTo = ".value";

    public OrdersPage(IPage page) : base(page) { }

    public async Task Navigate()
    {
        await Page.GotoAsync(Url);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task<bool> IsAtOrders()
    {
        return Page.Url.Contains("/orders") && 
               await Page.Locator(OrdersListing).IsVisibleAsync(new() { Timeout = 3000 });
    }

    public async Task<int> GetOrderCount()
    {
        return await Page.Locator(OrderCard).CountAsync();
    }

    public async Task<bool> HasOrderWithTotal(string expectedTotal)
    {
        var orderCount = await GetOrderCount();
        for (int i = 0; i < orderCount; i++)
        {
            var orderTotals = await Page.Locator(OrderCard).Nth(i).Locator(OrderTotal).AllTextContentsAsync();
            if (orderTotals.Any(t => t.Contains(expectedTotal.Replace("$", "").Trim())))
                return true;
        }
        return false;
    }

    public async Task<string> GetLatestOrderTotal()
    {
        if (await GetOrderCount() == 0) return "";
        var totals = await Page.Locator(OrderCard).First.Locator(OrderTotal).AllTextContentsAsync();
        return totals.FirstOrDefault(t => t.Contains("$") || char.IsDigit(t[0])) ?? "";
    }
}
