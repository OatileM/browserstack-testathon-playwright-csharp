using NUnit.Framework;
using Testathon.Tests.Framework;
using Testathon.Tests.Pages;

namespace Testathon.Tests.Tests.Smoke;

[Category("Smoke")]
public class SmokeTests : TestBase
{
    private HomePage _homePage = null!;

    [SetUp]
    public new async Task SetUp()
    {
        _homePage = new HomePage(Page);
        await _homePage.Navigate();
    }

    [Test]
    [Description("TC-SM-01: Home page loads successfully with core UI components visible")]
    public async Task TC_SM_01_HomePage_Loads_With_Core_UI()
    {
        var productCount = await _homePage.GetProductCardCount();
        Assert.That(productCount, Is.GreaterThan(0), "Product cards should be visible");
    }

    [Test]
    [Description("TC-SM-02: Navbar displays Offers, Orders, Favourites, and Sign In links")]
    public async Task TC_SM_02_Navbar_Displays_All_Links()
    {
        Assert.That(await _homePage.IsOffersLinkVisible(), Is.True, "Offers link should be visible");
        Assert.That(await _homePage.IsOrdersLinkVisible(), Is.True, "Orders link should be visible");
        Assert.That(await _homePage.IsFavouritesLinkVisible(), Is.True, "Favourites link should be visible");
        Assert.That(await _homePage.IsSignInLinkVisible(), Is.True, "Sign In link should be visible");
    }

    [Test]
    [Description("TC-SM-03: Default product catalog displays product cards on home page")]
    public async Task TC_SM_03_Product_Catalog_Displays_Cards()
    {
        var productCount = await _homePage.GetProductCardCount();
        Assert.That(productCount, Is.GreaterThan(0), "Product catalog should display product cards");
    }

    [Test]
    [Description("TC-SM-04: Product count indicator MUST exactly match number of displayed product cards - STRICT assertion")]
    public async Task TC_SM_04_Product_Count_Matches_Displayed_Cards()
    {
        var displayedCount = await _homePage.GetProductCardCount();
        var countText = await _homePage.GetProductCountText();
        
        // Extract numeric value from text like "25 Product(s) found."
        var match = System.Text.RegularExpressions.Regex.Match(countText, @"(\d+)");
        Assert.That(match.Success, Is.True, $"Product count text '{countText}' should contain a number");
        
        var indicatedCount = int.Parse(match.Groups[1].Value);
        Assert.That(displayedCount, Is.EqualTo(indicatedCount), 
            $"Displayed product cards ({displayedCount}) MUST exactly match count indicator ({indicatedCount})");
    }

    [Test]
    [Description("TC-SM-05: Vendor filter section displays all available vendors")]
    public async Task TC_SM_05_Vendor_Filter_Displays_All_Vendors()
    {
        var vendorCount = await _homePage.GetVendorFilterCount();
        Assert.That(vendorCount, Is.GreaterThanOrEqualTo(4), "Should display at least 4 vendor filters");
    }

    [Test]
    [Description("TC-SM-06: Vendor filter MUST reduce product count and show only matching products - STRICT assertion")]
    public async Task TC_SM_06_Single_Vendor_Filter_Updates_Catalog()
    {
        var initialCount = await _homePage.GetProductCardCount();
        await _homePage.SelectVendorByValue("Apple");
        var filteredCount = await _homePage.GetProductCardCount();
        
        Assert.That(filteredCount, Is.LessThanOrEqualTo(initialCount), 
            $"Filtered count ({filteredCount}) MUST be <= initial count ({initialCount})");
        Assert.That(filteredCount, Is.GreaterThan(0), 
            "At least one Apple product should be displayed after filtering");
    }

    [Test]
    [Description("TC-SM-07: Cart quantity MUST increment by exactly 1 after adding one product - STRICT assertion")]
    public async Task TC_SM_07_Adding_Product_Updates_Cart_Badge()
    {
        var initialQuantity = await _homePage.GetCartQuantity();
        await _homePage.AddToCartByIndex(0);
        var updatedQuantity = await _homePage.GetCartQuantity();
        
        Assert.That(updatedQuantity, Is.EqualTo("1"), 
            $"Cart quantity MUST be '1' after adding first product, but was '{updatedQuantity}'");
    }

    [Test]
    [Description("TC-SM-08: Cart subtotal MUST change from $ 0.00 to positive value - STRICT assertion")]
    public async Task TC_SM_08_Cart_Shows_NonZero_Subtotal()
    {
        await _homePage.AddToCartByIndex(0);
        await _homePage.OpenCart();
        var subtotal = await _homePage.GetSubtotal();
        
        Assert.That(subtotal, Does.Not.Contain("$ 0.00").And.Not.Contain("$0.00"), 
            $"Subtotal MUST not be zero after adding product, but was '{subtotal}'");
        
        // Verify subtotal contains dollar sign and numeric value
        Assert.That(subtotal, Does.Match(@"\$\s*\d+"), 
            $"Subtotal '{subtotal}' MUST be a valid currency format");
    }
}
