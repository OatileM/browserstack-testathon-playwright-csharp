using Microsoft.Playwright;

namespace Testathon.Tests.Pages;

public class HomePage : BasePage
{
    public const string Url = "https://testathon.live/";

    // Navbar
    private const string OffersLink = "#offers";
    private const string OrdersLink = "#orders";
    private const string FavouritesLink = "#favourites";
    private const string SignInLink = "text=Sign In";

    // Product catalog
    private const string ProductCards = "div.shelf-item";
    private const string ProductCount = ".products-found";
    private const string AddToCartButton = ".shelf-item__buy-btn";

    // Vendor filters
    private const string VendorCheckbox = "input[type='checkbox']";

    // Cart
    private const string CartIcon = ".bag";
    private const string CartQuantity = ".bag__quantity";
    private const string Subtotal = ".sub-price__val";
    private const string EmptyCartMessage = "p.shelf-empty";
    private const string ContinueShoppingButton = "text=Continue Shopping";

    // Favourites
    private const string FavouriteButton = ".shelf-item__fav";

    public HomePage(IPage page) : base(page) { }

    public async Task Navigate()
    {
        await Page.GotoAsync(Url);
    }

    // Navbar methods
    public async Task<bool> IsOffersLinkVisible() => await Page.Locator(OffersLink).IsVisibleAsync();
    public async Task<bool> IsOrdersLinkVisible() => await Page.Locator(OrdersLink).IsVisibleAsync();
    public async Task<bool> IsFavouritesLinkVisible() => await Page.Locator(FavouritesLink).IsVisibleAsync();
    public async Task<bool> IsSignInLinkVisible() => await Page.Locator(SignInLink).IsVisibleAsync();

    public async Task ClickOffersLink() => await Click(OffersLink);
    public async Task ClickOrdersLink() => await Click(OrdersLink);
    public async Task ClickFavouritesLink() => await Click(FavouritesLink);

    // Product catalog methods
    public async Task<int> GetProductCardCount()
    {
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        return await Page.Locator(ProductCards).CountAsync();
    }

    public async Task<string> GetProductCountText()
    {
        await WaitVisible(ProductCount);
        return await GetText(ProductCount);
    }

    // Vendor filter methods
    public async Task<int> GetVendorFilterCount()
    {
        return await Page.Locator(VendorCheckbox).CountAsync();
    }

    public async Task SelectVendorByValue(string vendor)
    {
        await Page.Locator($"input[type='checkbox'][value='{vendor}']").CheckAsync(new() { Force = true });
        await Page.WaitForTimeoutAsync(500);
    }

    // Get numeric value from product count indicator (e.g., "25 Product(s) found." -> 25)
    public async Task<int> GetProductCountIndicatorValue()
    {
        var countText = await GetProductCountText();
        var match = System.Text.RegularExpressions.Regex.Match(countText, @"(\d+)");
        return match.Success ? int.Parse(match.Groups[1].Value) : 0;
    }

    // Get vendor identifiers from all visible products
    // Based on HTML: products have data-sku attribute containing vendor info (e.g., "iPhone12-device-info.png", "samsung-S20-device-info.png")
    public async Task<List<string>> GetVisibleProductVendors()
    {
        var vendors = new List<string>();
        var productCount = await Page.Locator(ProductCards).CountAsync();
        
        for (int i = 0; i < productCount; i++)
        {
            var title = await Page.Locator(ProductCards).Nth(i).Locator(".shelf-item__title").TextContentAsync();
            vendors.Add(DetermineVendorFromTitle(title ?? ""));
        }
        
        return vendors;
    }

    // Determine vendor from product title based on known patterns
    private string DetermineVendorFromTitle(string title)
    {
        title = title.ToLower();
        if (title.Contains("iphone") || title.Contains("ipad")) return "Apple";
        if (title.Contains("galaxy") || title.Contains("samsung")) return "Samsung";
        if (title.Contains("pixel")) return "Google";
        if (title.Contains("oneplus")) return "OnePlus";
        return "Unknown";
    }

    public async Task SelectMultipleVendors(params string[] vendors)
    {
        foreach (var vendor in vendors)
        {
            await Page.Locator($"input[type='checkbox'][value='{vendor}']").CheckAsync(new() { Force = true });
        }
        await Page.WaitForTimeoutAsync(500);
    }

    public async Task ToggleVendorRapidly(string vendor, int times)
    {
        var checkbox = Page.Locator($"input[type='checkbox'][value='{vendor}']");
        for (int i = 0; i < times; i++)
        {
            if (await checkbox.IsCheckedAsync())
                await checkbox.UncheckAsync(new() { Force = true });
            else
                await checkbox.CheckAsync(new() { Force = true });
        }
        await Page.WaitForTimeoutAsync(500);
    }

    // Cart methods
    public async Task AddToCartByIndex(int index)
    {
        await Page.Locator(AddToCartButton).Nth(index).ClickAsync();
        await Page.WaitForTimeoutAsync(300);
    }

    public async Task OpenCart()
    {
        await Page.Locator(CartIcon).First.ClickAsync();
        await Page.WaitForTimeoutAsync(300);
    }

    public async Task CloseCart()
    {
        if (await IsCartOpen())
        {
            await Page.Locator(".float-cart__close-btn").ClickAsync();
            await Page.WaitForTimeoutAsync(300);
        }
    }

    public async Task<string> GetCartQuantity()
    {
        try
        {
            var quantity = await Page.Locator(CartQuantity).First.TextContentAsync(new() { Timeout = 5000 });
            return quantity?.Trim() ?? "0";
        }
        catch
        {
            return "0";
        }
    }

    public async Task<string> GetSubtotal()
    {
        await WaitVisible(Subtotal);
        return await GetText(Subtotal);
    }

    public async Task<bool> IsEmptyCartMessageVisible()
    {
        return await Page.Locator(EmptyCartMessage).IsVisibleAsync();
    }

    public async Task ClickContinueShopping()
    {
        await Click(ContinueShoppingButton);
        await Page.WaitForTimeoutAsync(300);
    }

    public async Task<bool> IsCartOpen()
    {
        return await Page.Locator(".float-cart--open").IsVisibleAsync();
    }

    // Favourites methods
    public async Task ClickFavouriteByIndex(int index)
    {
        await Page.Locator(FavouriteButton).Nth(index).ClickAsync();
        await Page.WaitForTimeoutAsync(300);
    }

    public async Task<string> GetProductTitleByIndex(int index)
    {
        return await Page.Locator(ProductCards).Nth(index).Locator(".shelf-item__title").TextContentAsync() ?? "";
    }
