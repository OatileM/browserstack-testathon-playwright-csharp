using Microsoft.Playwright;

namespace Testathon.Tests.Pages;

public class CheckoutPage : BasePage
{
    public const string Url = "https://testathon.live/checkout";

    // Form fields (from provided HTML)
    private const string FirstNameInput = "#firstNameInput";
    private const string LastNameInput = "#lastNameInput";
    private const string AddressInput = "#addressLine1Input";
    private const string ProvinceInput = "#provinceInput";
    private const string PostalCodeInput = "#postCodeInput";
    private const string SubmitButton = "#checkout-shipping-continue";

    // Order summary
    private const string OrderSummaryTotal = ".cart-priceItem--total .cart-priceItem-value";
    private const string OrderSummaryItems = ".cart-item";
    
    // Confirmation
    private const string ConfirmationMessage = "#confirmation-message";
    private const string OrderNumberElement = ".order-number, #order-number, [data-testid='order-number']";

    public CheckoutPage(IPage page) : base(page) { }

    public async Task Navigate()
    {
        await Page.GotoAsync(Url);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task<bool> IsAtCheckout()
    {
        // Check if we're on checkout page (not redirected to signin)
        return Page.Url.Contains("/checkout") && 
               await Page.Locator(FirstNameInput).IsVisibleAsync(new() { Timeout = 3000 }).ConfigureAwait(false);
    }

    public async Task<bool> IsRedirectedToSignIn()
    {
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        return Page.Url.Contains("signin") || Page.Url.Contains("?signin=true");
    }

    public async Task FillShippingInfo(string firstName, string lastName, string address, string province, string postalCode)
    {
        await Fill(FirstNameInput, firstName);
        await Fill(LastNameInput, lastName);
        await Fill(AddressInput, address);
        await Fill(ProvinceInput, province);
        await Fill(PostalCodeInput, postalCode);
    }

    public async Task<string> GetCheckoutTotal()
    {
        await WaitVisible(OrderSummaryTotal);
        return await GetText(OrderSummaryTotal);
    }

    public async Task<int> GetOrderSummaryItemCount()
    {
        return await Page.Locator(OrderSummaryItems).CountAsync();
    }

    public async Task<string> GetOrderSummaryItemTitle(int index)
    {
        return await Page.Locator(OrderSummaryItems).Nth(index).Locator(".cart-item-title, .product-name").TextContentAsync() ?? "";
    }

    public async Task SubmitOrder()
    {
        await Click(SubmitButton);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        // Wait for confirmation message to appear
        await Page.WaitForSelectorAsync(ConfirmationMessage, new() { Timeout = 10000 });
    }

    public async Task<bool> IsOrderConfirmed()
    {
        return await Page.Locator(ConfirmationMessage).IsVisibleAsync(new() { Timeout = 2000 }).ConfigureAwait(false);
    }

    public async Task<string> GetOrderNumber()
    {
        try
        {
            // Order number is in text like "Your order number is 23."
            var confirmationText = await Page.Locator(ConfirmationMessage).Locator("xpath=following-sibling::div").First.TextContentAsync(new() { Timeout = 5000 });
            var match = System.Text.RegularExpressions.Regex.Match(confirmationText ?? "", @"order number is\s+(\d+)");
            return match.Success ? match.Groups[1].Value : "";
        }
        catch
        {
            return "";
        }
    }

    // Check if required field validation is shown (HTML5 validation or custom)
    public async Task<bool> HasValidationError(string fieldId)
    {
        var field = Page.Locator(fieldId);
        
        // Check HTML5 validity
        var isInvalid = await field.EvaluateAsync<bool>("el => !el.validity.valid");
        if (isInvalid) return true;
        
        // Check for visible error message near field
        var errorMessage = Page.Locator($"{fieldId} ~ .form-field-error, {fieldId} + .form-field-error");
        return await errorMessage.IsVisibleAsync(new() { Timeout = 1000 }).ConfigureAwait(false);
    }

    public async Task<Dictionary<string, bool>> GetRequiredFieldValidationStatus()
    {
        return new Dictionary<string, bool>
        {
            { "FirstName", await HasValidationError(FirstNameInput) },
            { "LastName", await HasValidationError(LastNameInput) },
            { "Address", await HasValidationError(AddressInput) },
            { "Province", await HasValidationError(ProvinceInput) },
            { "PostalCode", await HasValidationError(PostalCodeInput) }
        };
    }
}
