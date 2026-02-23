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
        // Normalize expected total to just the numeric value
        var normalizedExpected = expectedTotal.Replace("$", "").Replace(" ", "").Replace(",", "").Replace(".00", "").Trim();
        
        var orderCount = await GetOrderCount();
        for (int i = 0; i < orderCount; i++)
        {
            // Get all text from order card
            var orderText = await Page.Locator(OrderCard).Nth(i).TextContentAsync();
            if (orderText != null)
            {
                // Normalize order text and check for match (with or without .00)
                var normalizedOrderText = orderText.Replace("$", "").Replace(" ", "").Replace(",", "");
                // Check both with and without decimal
                if (normalizedOrderText.Contains(normalizedExpected) || 
                    normalizedOrderText.Contains(normalizedExpected + ".00"))
                    return true;
            }
        }
        return false;
    }
}
