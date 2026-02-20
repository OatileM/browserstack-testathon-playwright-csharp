using Microsoft.Playwright;
using NUnit.Framework;
using Testathon.Tests.Framework;
using Testathon.Tests.Pages;

namespace Testathon.Tests.Tests.Negative;

[Category("Negative")]
public class CheckoutAccessTests : TestBase
{
    private HomePage _homePage = null!;
    private CheckoutPage _checkoutPage = null!;

    [SetUp]
    public new async Task SetUp()
    {
        _homePage = new HomePage(Page);
        _checkoutPage = new CheckoutPage(Page);
        await _homePage.Navigate();
    }

    [Test]
    [Description("TC-NEG-AUTH-07: User MUST NOT access checkout without authentication - STRICT assertion")]
    public async Task TC_NEG_AUTH_07_Cannot_Checkout_Without_Login()
    {
        // Add product to cart
        await _homePage.AddToCartByIndex(0);
        var cartQuantity = await _homePage.GetCartQuantity();
        Assert.That(cartQuantity, Is.EqualTo("1"), "Cart must have 1 item before attempting checkout");
        
        // Attempt to navigate to checkout
        await _checkoutPage.Navigate();
        
        // Strict assertion: Must be redirected to signin
        var isRedirected = await _checkoutPage.IsRedirectedToSignIn();
        Assert.That(isRedirected, Is.True, 
            $"User MUST be redirected to signin when accessing checkout without authentication. Current URL: {Page.Url}");
    }
}
