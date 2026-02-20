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
    
    // Confirmation
    private const string ConfirmationMessage = "#confirmation-message";

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

    public async Task<string> GetCheckoutTotal()
    {
        await WaitVisible(OrderSummaryTotal);
        return await GetText(OrderSummaryTotal);
    }

    public async Task SubmitOrder()
    {
        await Click(SubmitButton);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task<bool> IsOrderConfirmed()
    {
        return await Page.Locator(ConfirmationMessage).IsVisibleAsync(new() { Timeout = 5000 }).ConfigureAwait(false);
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
