using NUnit.Framework;
using Testathon.Tests.Framework;
using Testathon.Tests.Pages;

namespace Testathon.Tests.Tests.Regression;

[Category("Regression")]
public class RegressionTests : TestBase
{
    private HomePage _homePage = null!;

    [SetUp]
    public new async Task SetUp()
    {
        _homePage = new HomePage(Page);
        await _homePage.Navigate();
    }

    [Test]
    [Description("TC-REG-17: Product card MUST contain all required elements with non-empty content - STRICT assertion")]
    public async Task TC_REG_17_Product_Card_Shows_All_Elements()
    {
        var productCount = await _homePage.GetProductCardCount();
        Assert.That(productCount, Is.GreaterThan(0), "At least one product card MUST be present");
        
        var hasImage = await Page.Locator("div.shelf-item img").First.IsVisibleAsync();
        var hasTitle = await Page.Locator("div.shelf-item .shelf-item__title").First.IsVisibleAsync();
        var hasPrice = await Page.Locator("div.shelf-item .shelf-item__price").First.IsVisibleAsync();
        var hasButton = await Page.Locator("div.shelf-item .shelf-item__buy-btn").First.IsVisibleAsync();
        
        Assert.That(hasImage, Is.True, "Product card MUST have visible image");
        Assert.That(hasTitle, Is.True, "Product card MUST have visible title");
        Assert.That(hasPrice, Is.True, "Product card MUST have visible price");
        Assert.That(hasButton, Is.True, "Product card MUST have visible Add to cart button");
        
        // Verify content is not empty
        var titleText = await Page.Locator("div.shelf-item .shelf-item__title").First.TextContentAsync();
        var priceText = await Page.Locator("div.shelf-item .shelf-item__price").First.TextContentAsync();
        
        Assert.That(titleText?.Trim(), Is.Not.Empty, "Product title MUST not be empty");
        Assert.That(priceText?.Trim(), Is.Not.Empty, "Product price MUST not be empty");
    }

    [Test]
    [Description("TC-REG-18: Cart quantity MUST increment by 2 and subtotal MUST increase - STRICT assertion")]
    public async Task TC_REG_18_Multiple_Products_Update_Cart()
    {
        await _homePage.AddToCartByIndex(0);
        var quantityAfterFirst = await _homePage.GetCartQuantity();
        
        await _homePage.AddToCartByIndex(1);
        var quantityAfterSecond = await _homePage.GetCartQuantity();
        
        Assert.That(quantityAfterFirst, Is.EqualTo("1"), 
            $"Cart quantity MUST be '1' after adding first product, but was '{quantityAfterFirst}'");
        Assert.That(quantityAfterSecond, Is.EqualTo("2"), 
            $"Cart quantity MUST be '2' after adding second product, but was '{quantityAfterSecond}'");
        
        await _homePage.OpenCart();
        var subtotal = await _homePage.GetSubtotal();
        
        Assert.That(subtotal, Does.Not.Contain("$ 0.00").And.Not.Contain("$0.00"), 
            $"Subtotal MUST not be zero with 2 items in cart, but was '{subtotal}'");
    }

    [Test]
    [Description("TC-REG-19: Cart quantity MUST persist exactly after navigation - STRICT assertion. May expose state management bug.")]
    public async Task TC_REG_19_Cart_State_Persists_After_Navigation()
    {
        await _homePage.AddToCartByIndex(0);
        var initialQuantity = await _homePage.GetCartQuantity();
        
        Assert.That(initialQuantity, Is.EqualTo("1"), 
            $"Initial cart quantity MUST be '1', but was '{initialQuantity}'");
        
        await _homePage.ClickOffersLink();
        await Page.WaitForTimeoutAsync(500);
        await _homePage.Navigate();
        
        var quantityAfterNav = await _homePage.GetCartQuantity();
        Assert.That(quantityAfterNav, Is.EqualTo(initialQuantity), 
            $"Cart quantity MUST persist after navigation. Expected '{initialQuantity}' but got '{quantityAfterNav}'. This may indicate a state management bug.");
    }

    [Test]
    [Description("TC-REG-20: Rapid user interactions do not crash UI")]
    public async Task TC_REG_20_Rapid_Interactions_Do_Not_Crash()
    {
        await _homePage.AddToCartByIndex(0);
        await _homePage.SelectVendorByValue("Apple");
        await _homePage.AddToCartByIndex(0);
        
        var productCount = await _homePage.GetProductCardCount();
        Assert.That(productCount, Is.GreaterThan(0), "UI should remain stable after rapid interactions");
    }
}
