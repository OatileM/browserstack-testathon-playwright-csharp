using Microsoft.Playwright;

namespace Testathon.Tests.Pages;

public class FavouritesPage : BasePage
{
    public const string Url = "https://testathon.live/favourites";

    private const string FavouritesListing = ".favourites-listing, .shelf";
    private const string ProductCards = "div.shelf-item";
    private const string ProductTitle = ".shelf-item__title";

    public FavouritesPage(IPage page) : base(page) { }

    public async Task Navigate()
    {
        await Page.GotoAsync(Url);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task<bool> IsAtFavourites()
    {
        return Page.Url.Contains("/favourites");
    }

    public async Task<int> GetFavouriteCount()
    {
        return await Page.Locator(ProductCards).CountAsync();
    }

    public async Task<bool> HasProductWithTitle(string expectedTitle)
    {
        var productCount = await GetFavouriteCount();
        for (int i = 0; i < productCount; i++)
        {
            var title = await Page.Locator(ProductCards).Nth(i).Locator(ProductTitle).TextContentAsync();
            if (title != null && title.Trim().Equals(expectedTitle.Trim(), StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }
}
