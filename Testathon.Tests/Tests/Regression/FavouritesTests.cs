using Microsoft.Playwright;
using NUnit.Framework;
using Testathon.Tests.Framework;
using Testathon.Tests.Pages;

namespace Testathon.Tests.Tests.Regression;

[Category("Regression")]
public class FavouritesTests : TestBase
{
    private HomePage _homePage = null!;
    private SignInPage _signInPage = null!;
    private FavouritesPage _favouritesPage = null!;

    [SetUp]
    public new async Task SetUp()
    {
        _homePage = new HomePage(Page);
        _signInPage = new SignInPage(Page);
        _favouritesPage = new FavouritesPage(Page);
        await _homePage.Navigate();
    }

    [Test]
    [Description("TC-REG-FAV-01: Add product to favourites and verify it appears in favourites page")]
    public async Task TC_REG_FAV_01_Add_Product_To_Favourites_And_Verify()
    {
        // Authenticate first (required for favourites)
        await _signInPage.Navigate();
        await _signInPage.Login("demouser", "testingisfun99");
        await _homePage.Navigate();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Get product title before adding to favourites
        var productTitle = await _homePage.GetProductTitleByIndex(0);
        Assert.That(productTitle, Is.Not.Empty, "Product title must not be empty");

        // Click favourite button on first product
        await _homePage.ClickFavouriteByIndex(0);

        // Navigate to favourites page
        await _homePage.ClickFavouritesLink();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Verify at favourites page
        var isAtFavourites = await _favouritesPage.IsAtFavourites();
        Assert.That(isAtFavourites, Is.True, "Must be at favourites page");

        // Verify favourited product appears in list
        var hasFavourite = await _favouritesPage.HasProductWithTitle(productTitle);
        Assert.That(hasFavourite, Is.True, 
            $"Favourites page must contain product '{productTitle}'");

        // Verify at least one favourite exists
        var favouriteCount = await _favouritesPage.GetFavouriteCount();
        Assert.That(favouriteCount, Is.GreaterThan(0), 
            "At least one product must be displayed in favourites list");
    }
}
