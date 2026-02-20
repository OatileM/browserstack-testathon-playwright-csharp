using Microsoft.Playwright;
using NUnit.Framework;
using Testathon.Tests.Framework;
using Testathon.Tests.Pages;

namespace Testathon.Tests.Tests.Negative;

[Category("Negative")]
public class CheckoutValidationTests : TestBase
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
    [Description("TC-NEG-CO-02: Cannot submit order with empty required fields - STRICT validation assertion")]
    public async Task TC_NEG_CO_02_Cannot_Submit_Order_With_Empty_Fields()
    {
        // Add product to cart
        await _homePage.AddToCartByIndex(0);
        
        // Navigate to checkout
        await _checkoutPage.Navigate();
        
        // Gate: If redirected to signin, skip this test
        if (await _checkoutPage.IsRedirectedToSignIn())
        {
            Assert.Ignore("Checkout requires authentication. Test TC-NEG-AUTH-07 covers this scenario.");
            return;
        }
        
        // Verify we're at checkout
        var isAtCheckout = await _checkoutPage.IsAtCheckout();
        Assert.That(isAtCheckout, Is.True, "Must be at checkout page to test validation");
        
        // Attempt to submit with empty fields
        await _checkoutPage.SubmitOrder();
        
        // Strict assertion: Order should NOT be confirmed
        var isConfirmed = await _checkoutPage.IsOrderConfirmed();
        Assert.That(isConfirmed, Is.False, 
            "Order MUST NOT be confirmed when required fields are empty");
        
        // Verify validation errors are shown for required fields
        var validationStatus = await _checkoutPage.GetRequiredFieldValidationStatus();
        
        Assert.That(validationStatus["FirstName"], Is.True, 
            "First Name field MUST show validation error when empty");
        Assert.That(validationStatus["LastName"], Is.True, 
            "Last Name field MUST show validation error when empty");
        Assert.That(validationStatus["Address"], Is.True, 
            "Address field MUST show validation error when empty");
        Assert.That(validationStatus["Province"], Is.True, 
            "Province field MUST show validation error when empty");
        Assert.That(validationStatus["PostalCode"], Is.True, 
            "Postal Code field MUST show validation error when empty");
    }
}
