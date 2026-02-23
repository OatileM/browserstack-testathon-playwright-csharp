using Microsoft.Playwright;
using NUnit.Framework;
using Testathon.Tests.Framework;
using Testathon.Tests.Pages;

namespace Testathon.Tests.Tests.Regression;

[Category("Regression")]
public class CheckoutTotalsTests : TestBase
{
    private HomePage _homePage = null!;
    private SignInPage _signInPage = null!;
    private CheckoutPage _checkoutPage = null!;

    [SetUp]
    public new async Task SetUp()
    {
        _homePage = new HomePage(Page);
        _signInPage = new SignInPage(Page);
        _checkoutPage = new CheckoutPage(Page);
        await _homePage.Navigate();
    }

    [Test]
    [Description("TC-REG-CO-03: Checkout total MUST exactly match cart total - STRICT numeric equality")]
    public async Task TC_REG_CO_03_Checkout_Total_Matches_Cart_Total()
    {
        // Add product to cart
        await _homePage.AddToCartByIndex(0);
        await _homePage.OpenCart();
        
        // Capture cart subtotal
        var cartSubtotal = await _homePage.GetSubtotal();
        
        // Navigate to checkout
        await _checkoutPage.Navigate();
        
        // If redirected to signin, authenticate and retry
        if (await _checkoutPage.IsRedirectedToSignIn())
        {
            await _signInPage.Login("demouser", "testingisfun99");
            await _checkoutPage.Navigate();
        }
        
        // Verify we're at checkout
        var isAtCheckout = await _checkoutPage.IsAtCheckout();
        Assert.That(isAtCheckout, Is.True, "Must be at checkout page to compare totals");
        
        // Capture checkout total
        var checkoutTotal = await _checkoutPage.GetCheckoutTotal();
        
        // Normalize both values (remove currency symbols, spaces, commas)
        var cartValue = NormalizePrice(cartSubtotal);
        var checkoutValue = NormalizePrice(checkoutTotal);
        
        // Strict assertion: Values must match exactly
        Assert.That(checkoutValue, Is.EqualTo(cartValue), 
            $"Checkout total ({checkoutTotal} = {checkoutValue}) MUST exactly match cart subtotal ({cartSubtotal} = {cartValue})");
    }

    [Test]
    [Description("TC-REG-CO-04: Multiple items checkout total MUST match cart total - STRICT numeric equality")]
    public async Task TC_REG_CO_04_Multiple_Items_Checkout_Total_Matches_Cart_Total()
    {
        // Add multiple products to cart
        await _homePage.AddToCartByIndex(0);
        await _homePage.CloseCart();
        await _homePage.AddToCartByIndex(1);
        await _homePage.CloseCart();
        await _homePage.AddToCartByIndex(2);
        await _homePage.OpenCart();
        
        // Capture cart subtotal
        var cartSubtotal = await _homePage.GetSubtotal();
        
        // Navigate to checkout
        await _checkoutPage.Navigate();
        
        // If redirected to signin, authenticate and retry
        if (await _checkoutPage.IsRedirectedToSignIn())
        {
            await _signInPage.Login("demouser", "testingisfun99");
            await _checkoutPage.Navigate();
        }
        
        // Verify we're at checkout
        var isAtCheckout = await _checkoutPage.IsAtCheckout();
        Assert.That(isAtCheckout, Is.True, "Must be at checkout page to compare totals");
        
        // Capture checkout total
        var checkoutTotal = await _checkoutPage.GetCheckoutTotal();
        
        // Normalize both values
        var cartValue = NormalizePrice(cartSubtotal);
        var checkoutValue = NormalizePrice(checkoutTotal);
        
        // Strict assertion: Values must match exactly
        Assert.That(checkoutValue, Is.EqualTo(cartValue), 
            $"Checkout total ({checkoutTotal} = {checkoutValue}) MUST exactly match cart subtotal ({cartSubtotal} = {cartValue}) for multiple items");
    }

    [Test]
    [Description("TC-REG-CO-05: Multiple vendors checkout validates cart total - STRICT numeric equality")]
    public async Task TC_REG_CO_05_Multiple_Vendors_Checkout_Total_Matches_Cart_Total()
    {
        // Filter to Apple products and add one
        await _homePage.SelectVendorByValue("Apple");
        await _homePage.AddToCartByIndex(0);
        await _homePage.CloseCart();
        
        // Filter to Samsung products and add one
        await _homePage.SelectVendorByValue("Samsung");
        await _homePage.AddToCartByIndex(0);
        await _homePage.OpenCart();
        
        // Capture cart subtotal
        var cartSubtotal = await _homePage.GetSubtotal();
        
        // Navigate to checkout
        await _checkoutPage.Navigate();
        
        // If redirected to signin, authenticate and retry
        if (await _checkoutPage.IsRedirectedToSignIn())
        {
            await _signInPage.Login("demouser", "testingisfun99");
            await _checkoutPage.Navigate();
        }
        
        // Verify we're at checkout
        var isAtCheckout = await _checkoutPage.IsAtCheckout();
        Assert.That(isAtCheckout, Is.True, "Must be at checkout page to compare totals");
        
        // Capture checkout total
        var checkoutTotal = await _checkoutPage.GetCheckoutTotal();
        
        // Normalize both values
        var cartValue = NormalizePrice(cartSubtotal);
        var checkoutValue = NormalizePrice(checkoutTotal);
        
        // Strict assertion: Values must match exactly
        Assert.That(checkoutValue, Is.EqualTo(cartValue), 
            $"Checkout total ({checkoutTotal} = {checkoutValue}) MUST exactly match cart subtotal ({cartSubtotal} = {cartValue}) for multiple vendors");
    }

    private decimal NormalizePrice(string priceText)
    {
        // Remove currency symbols, spaces, and parse to decimal
        var normalized = priceText
            .Replace("$", "")
            .Replace("USD", "")
            .Replace(" ", "")
            .Replace(",", "")
            .Trim();
        
        return decimal.Parse(normalized, System.Globalization.CultureInfo.InvariantCulture);
    }
}
