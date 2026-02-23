using Microsoft.Playwright;
using NUnit.Framework;
using Testathon.Tests.Framework;
using Testathon.Tests.Pages;

namespace Testathon.Tests.Tests.Regression;

[Category("Regression")]
public class PurchaseFlowTests : TestBase
{
    private HomePage _homePage = null!;
    private SignInPage _signInPage = null!;
    private CheckoutPage _checkoutPage = null!;
    private OrdersPage _ordersPage = null!;

    [SetUp]
    public new async Task SetUp()
    {
        _homePage = new HomePage(Page);
        _signInPage = new SignInPage(Page);
        _checkoutPage = new CheckoutPage(Page);
        _ordersPage = new OrdersPage(Page);
        await _homePage.Navigate();
    }

    [Test]
    [Description("TC-REG-PUR-01: Complete purchase flow - add product, checkout, fill shipping, validate order summary, submit, verify order appears in orders page")]
    public async Task TC_REG_PUR_01_Complete_Single_Product_Purchase_Flow()
    {
        // Add product to cart
        await _homePage.AddToCartByIndex(0);
        await _homePage.OpenCart();
        var cartSubtotal = await _homePage.GetSubtotal();
        await _homePage.CloseCart();

        // Navigate to checkout
        await _checkoutPage.Navigate();

        // Authenticate if needed
        if (await _checkoutPage.IsRedirectedToSignIn())
        {
            await _signInPage.Login("demouser", "testingisfun99");
            await _checkoutPage.Navigate();
        }

        // Verify at checkout
        var isAtCheckout = await _checkoutPage.IsAtCheckout();
        Assert.That(isAtCheckout, Is.True, "Must be at checkout page");

        // Validate order summary reflects cart total
        var checkoutTotal = await _checkoutPage.GetCheckoutTotal();
        Assert.That(NormalizePrice(checkoutTotal), Is.EqualTo(NormalizePrice(cartSubtotal)),
            $"Order summary total ({checkoutTotal}) must match cart subtotal ({cartSubtotal})");

        // Fill shipping information
        await _checkoutPage.FillShippingInfo("John", "Doe", "123 Test St", "Gauteng", "2000");

        // Submit order
        await _checkoutPage.SubmitOrder();

        // Verify order confirmation
        var isConfirmed = await _checkoutPage.IsOrderConfirmed();
        Assert.That(isConfirmed, Is.True, "Order confirmation message must be displayed");

        // Navigate to orders page
        await _ordersPage.Navigate();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Verify order appears in orders list
        var isAtOrders = await _ordersPage.IsAtOrders();
        Assert.That(isAtOrders, Is.True, "Must be at orders page");

        var orderCount = await _ordersPage.GetOrderCount();
        Assert.That(orderCount, Is.GreaterThan(0), "At least one order must be displayed in orders list");

        // Verify latest order has matching total
        var hasMatchingOrder = await _ordersPage.HasOrderWithTotal(cartSubtotal);
        Assert.That(hasMatchingOrder, Is.True, 
            $"Orders page must contain order with total {cartSubtotal}");
    }

    [Test]
    [Description("TC-REG-PUR-02: Complete purchase flow with multiple vendor products - validate order summary and orders page")]
    public async Task TC_REG_PUR_02_Complete_Multiple_Vendor_Purchase_Flow()
    {
        // Add Apple product
        await _homePage.SelectVendorByValue("Apple");
        await _homePage.AddToCartByIndex(0);
        await _homePage.CloseCart();

        // Add Samsung product
        await _homePage.SelectVendorByValue("Samsung");
        await _homePage.AddToCartByIndex(0);
        await _homePage.OpenCart();
        var cartSubtotal = await _homePage.GetSubtotal();
        await _homePage.CloseCart();

        // Navigate to checkout
        await _checkoutPage.Navigate();

        // Authenticate if needed
        if (await _checkoutPage.IsRedirectedToSignIn())
        {
            await _signInPage.Login("demouser", "testingisfun99");
            await _checkoutPage.Navigate();
        }

        // Verify at checkout
        var isAtCheckout = await _checkoutPage.IsAtCheckout();
        Assert.That(isAtCheckout, Is.True, "Must be at checkout page");

        // Validate order summary reflects cart total
        var checkoutTotal = await _checkoutPage.GetCheckoutTotal();
        Assert.That(NormalizePrice(checkoutTotal), Is.EqualTo(NormalizePrice(cartSubtotal)),
            $"Order summary total ({checkoutTotal}) must match cart subtotal ({cartSubtotal}) for multiple vendors");

        // Validate order summary has multiple items
        var itemCount = await _checkoutPage.GetOrderSummaryItemCount();
        Assert.That(itemCount, Is.GreaterThanOrEqualTo(2), 
            "Order summary must show at least 2 items for multiple vendor purchase");

        // Fill shipping information
        await _checkoutPage.FillShippingInfo("Jane", "Smith", "456 Test Ave", "Western Cape", "8000");

        // Submit order
        await _checkoutPage.SubmitOrder();

        // Verify order confirmation
        var isConfirmed = await _checkoutPage.IsOrderConfirmed();
        Assert.That(isConfirmed, Is.True, "Order confirmation message must be displayed");

        // Navigate to orders page
        await _ordersPage.Navigate();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Verify order appears in orders list
        var isAtOrders = await _ordersPage.IsAtOrders();
        Assert.That(isAtOrders, Is.True, "Must be at orders page");

        var orderCount = await _ordersPage.GetOrderCount();
        Assert.That(orderCount, Is.GreaterThan(0), "At least one order must be displayed in orders list");

        // Verify latest order has matching total
        var hasMatchingOrder = await _ordersPage.HasOrderWithTotal(cartSubtotal);
        Assert.That(hasMatchingOrder, Is.True, 
            $"Orders page must contain order with total {cartSubtotal} for multiple vendor purchase");
    }

    private decimal NormalizePrice(string priceText)
    {
        var normalized = priceText
            .Replace("$", "")
            .Replace("USD", "")
            .Replace(" ", "")
            .Replace(",", "")
            .Trim();
        
        return decimal.Parse(normalized, System.Globalization.CultureInfo.InvariantCulture);
    }
}
