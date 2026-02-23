using Microsoft.Playwright;
using NUnit.Framework;
using Testathon.Tests.Framework;
using Testathon.Tests.Pages;

namespace Testathon.Tests.Tests.Negative;

[Category("Negative")]
public class NegativeTests : TestBase
{
    private HomePage _homePage = null!;

    [SetUp]
    public new async Task SetUp()
    {
        _homePage = new HomePage(Page);
        await _homePage.Navigate();
    }

    [Test]
    [Description("TC-NEG-09: Offers link MUST navigate to /offers URL - STRICT routing assertion. May expose routing bug.")]
    public async Task TC_NEG_09_Offers_Link_Navigates()
    {
        var signInPage = new SignInPage(Page);
        await _homePage.ClickOffersLink();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        if (Page.Url.Contains("signin"))
        {
            await signInPage.Login("demouser", "testingisfun99");
            await Page.WaitForURLAsync(url => url.Contains("/offers"), new() { Timeout = 5000 }).ContinueWith(_ => Task.CompletedTask);
        }
        
        Assert.That(Page.Url, Does.Contain("/offers"), 
            $"URL MUST contain '/offers' after clicking Offers link, but was '{Page.Url}'. This may indicate a routing bug.");
    }

    [Test]
    [Description("TC-NEG-10: Orders link MUST navigate to /orders URL - STRICT routing assertion. May expose routing bug.")]
    public async Task TC_NEG_10_Orders_Link_Navigates()
    {
        var signInPage = new SignInPage(Page);
        await _homePage.ClickOrdersLink();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        if (Page.Url.Contains("signin"))
        {
            await signInPage.Login("demouser", "testingisfun99");
            await Page.WaitForURLAsync(url => url.Contains("/orders"), new() { Timeout = 5000 }).ContinueWith(_ => Task.CompletedTask);
        }
        
        Assert.That(Page.Url, Does.Contain("/orders"), 
            $"URL MUST contain '/orders' after clicking Orders link, but was '{Page.Url}'. This may indicate a routing bug.");
    }

    [Test]
    [Description("TC-NEG-11: Favourites link MUST navigate to /favourites URL - STRICT routing assertion. May expose routing bug.")]
    public async Task TC_NEG_11_Favourites_Link_Navigates()
    {
        var signInPage = new SignInPage(Page);
        await _homePage.ClickFavouritesLink();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        if (Page.Url.Contains("signin"))
        {
            await signInPage.Login("demouser", "testingisfun99");
            await Page.WaitForURLAsync(url => url.Contains("/favourites"), new() { Timeout = 5000 }).ContinueWith(_ => Task.CompletedTask);
        }
        
        Assert.That(Page.Url, Does.Contain("/favourites"), 
            $"URL MUST contain '/favourites' after clicking Favourites link, but was '{Page.Url}'. This may indicate a routing bug.");
    }

    [Test]
    [Description("TC-NEG-15: Empty cart MUST show exact empty message and $ 0.00 subtotal - STRICT assertion")]
    public async Task TC_NEG_15_Empty_Cart_Shows_Empty_State()
    {
        await _homePage.OpenCart();
        var isEmpty = await _homePage.IsEmptyCartMessageVisible();
        var subtotal = await _homePage.GetSubtotal();
        
        Assert.That(isEmpty, Is.True, "Empty cart message MUST be visible when cart is empty");
        Assert.That(subtotal, Does.Contain("$ 0.00").Or.Contain("$0.00"), 
            $"Subtotal MUST be exactly '$ 0.00' for empty cart, but was '{subtotal}'");
    }

    [Test]
    [Description("TC-NEG-16: Continue Shopping closes cart and restores browsing")]
    public async Task TC_NEG_16_Continue_Shopping_Closes_Cart()
    {
        await _homePage.OpenCart();
        await _homePage.ClickContinueShopping();
        var isCartOpen = await _homePage.IsCartOpen();
        Assert.That(isCartOpen, Is.False, "Cart should be closed after Continue Shopping");
    }


    [Test]
    [Description("TC-NEG-14: Rapid toggling vendor filter does not break final state")]
    public async Task TC_NEG_14_Rapid_Toggle_Does_Not_Break_State()
    {
        await _homePage.ToggleVendorRapidly("Apple", 5);
        var productCount = await _homePage.GetProductCardCount();
        Assert.That(productCount, Is.GreaterThan(0), "UI should remain stable after rapid toggling");
    }
}
