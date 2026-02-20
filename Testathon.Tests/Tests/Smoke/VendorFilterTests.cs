using NUnit.Framework;
using Testathon.Tests.Framework;
using Testathon.Tests.Pages;

namespace Testathon.Tests.Tests.Smoke;

[Category("Smoke")]
public class VendorFilterTests : TestBase
{
    private HomePage _homePage = null!;

    [SetUp]
    public new async Task SetUp()
    {
        _homePage = new HomePage(Page);
        await _homePage.Navigate();
    }

    [Test]
    [Description("TC-SM-VEND-01: Vendor filter Apple MUST show ONLY Apple products - STRICT assertion")]
    public async Task TC_SM_VEND_01_Apple_Filter_Shows_Only_Apple_Products()
    {
        var initialCount = await _homePage.GetProductCardCount();
        var initialIndicator = await _homePage.GetProductCountIndicatorValue();
        
        await _homePage.SelectVendorByValue("Apple");
        
        var filteredCount = await _homePage.GetProductCardCount();
        var filteredIndicator = await _homePage.GetProductCountIndicatorValue();
        var vendors = await _homePage.GetVisibleProductVendors();
        
        // Strict assertions
        Assert.That(filteredCount, Is.LessThanOrEqualTo(initialCount), 
            $"Filtered count ({filteredCount}) MUST be <= initial count ({initialCount})");
        Assert.That(filteredCount, Is.EqualTo(filteredIndicator), 
            $"Product count indicator ({filteredIndicator}) MUST exactly match displayed cards ({filteredCount})");
        Assert.That(filteredCount, Is.GreaterThan(0), 
            "At least one Apple product MUST be displayed");
        
        // Verify ALL products are Apple
        foreach (var vendor in vendors)
        {
            Assert.That(vendor, Is.EqualTo("Apple"), 
                $"ALL visible products MUST be Apple, but found '{vendor}'");
        }
    }

    [Test]
    [Description("TC-SM-VEND-02: Vendor filter Samsung MUST show ONLY Samsung products - STRICT assertion")]
    public async Task TC_SM_VEND_02_Samsung_Filter_Shows_Only_Samsung_Products()
    {
        var initialCount = await _homePage.GetProductCardCount();
        
        await _homePage.SelectVendorByValue("Samsung");
        
        var filteredCount = await _homePage.GetProductCardCount();
        var filteredIndicator = await _homePage.GetProductCountIndicatorValue();
        var vendors = await _homePage.GetVisibleProductVendors();
        
        // Strict assertions
        Assert.That(filteredCount, Is.LessThanOrEqualTo(initialCount), 
            $"Filtered count ({filteredCount}) MUST be <= initial count ({initialCount})");
        Assert.That(filteredCount, Is.EqualTo(filteredIndicator), 
            $"Product count indicator ({filteredIndicator}) MUST exactly match displayed cards ({filteredCount})");
        Assert.That(filteredCount, Is.GreaterThan(0), 
            "At least one Samsung product MUST be displayed");
        
        // Verify ALL products are Samsung
        foreach (var vendor in vendors)
        {
            Assert.That(vendor, Is.EqualTo("Samsung"), 
                $"ALL visible products MUST be Samsung, but found '{vendor}'");
        }
    }

    [Test]
    [Description("TC-SM-VEND-03: Vendor filter Google MUST show ONLY Google products - STRICT assertion")]
    public async Task TC_SM_VEND_03_Google_Filter_Shows_Only_Google_Products()
    {
        var initialCount = await _homePage.GetProductCardCount();
        
        await _homePage.SelectVendorByValue("Google");
        
        var filteredCount = await _homePage.GetProductCardCount();
        var filteredIndicator = await _homePage.GetProductCountIndicatorValue();
        var vendors = await _homePage.GetVisibleProductVendors();
        
        // Strict assertions
        Assert.That(filteredCount, Is.LessThanOrEqualTo(initialCount), 
            $"Filtered count ({filteredCount}) MUST be <= initial count ({initialCount})");
        Assert.That(filteredCount, Is.EqualTo(filteredIndicator), 
            $"Product count indicator ({filteredIndicator}) MUST exactly match displayed cards ({filteredCount})");
        Assert.That(filteredCount, Is.GreaterThan(0), 
            "At least one Google product MUST be displayed");
        
        // Verify ALL products are Google
        foreach (var vendor in vendors)
        {
            Assert.That(vendor, Is.EqualTo("Google"), 
                $"ALL visible products MUST be Google, but found '{vendor}'");
        }
    }

    [Test]
    [Description("TC-NEG-VEND-04: Vendor filter OnePlus behavior validation - STRICT assertion. May expose data/filter bug.")]
    public async Task TC_NEG_VEND_04_OnePlus_Filter_Behavior()
    {
        var initialCount = await _homePage.GetProductCardCount();
        
        await _homePage.SelectVendorByValue("OnePlus");
        
        var filteredCount = await _homePage.GetProductCardCount();
        var filteredIndicator = await _homePage.GetProductCountIndicatorValue();
        
        // Strict assertions
        Assert.That(filteredCount, Is.LessThanOrEqualTo(initialCount), 
            $"Filtered count ({filteredCount}) MUST be <= initial count ({initialCount})");
        Assert.That(filteredCount, Is.EqualTo(filteredIndicator), 
            $"Product count indicator ({filteredIndicator}) MUST exactly match displayed cards ({filteredCount})");
        
        // OnePlus may have 0 products (no inventory) OR only OnePlus products
        if (filteredCount > 0)
        {
            var vendors = await _homePage.GetVisibleProductVendors();
            foreach (var vendor in vendors)
            {
                Assert.That(vendor, Is.EqualTo("OnePlus"), 
                    $"If OnePlus products exist, ALL MUST be OnePlus, but found '{vendor}'. This may indicate a filter or data bug.");
            }
        }
        // If filteredCount == 0, that's acceptable (no OnePlus inventory)
    }
}
