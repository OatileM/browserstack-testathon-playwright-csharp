using Microsoft.Playwright;

namespace Testathon.Tests.Pages;

public class OrdersPage : BasePage
{
    public const string Url = "https://testathon.live/orders";

    private const string OrdersListing = ".orders-listing";
    private const string OrderCard = ".order";
    private const string OrderPriceItem = ".cart-priceItem-value";

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
        // Normalize expected total (remove $, spaces, etc)
        var normalizedExpected = expectedTotal.Replace("$", "").Replace(" ", "").Replace(",", "").Trim();
        
        var orderCount = await GetOrderCount();
        for (int i = 0; i < orderCount; i++)
        {
            // Get all text from order card that might contain price
            var orderText = await Page.Locator(OrderCard).Nth(i).TextContentAsync();
            if (orderText != null)
            {
                // Check if normalized expected total appears in order text
                var normalizedOrderText = orderText.Replace("$", "").Replace(" ", "").Replace(",", "");
                if (normalizedOrderText.Contains(normalizedExpected))
                    return true;
            }
        }
        return false;
    }
}
