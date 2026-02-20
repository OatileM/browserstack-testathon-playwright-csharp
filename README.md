# Testathon – Playwright + C# (NUnit)

Production-quality test automation framework built for the **BrowserStack Testathon – Hands-on Testing Challenge (Johannesburg)**.

Uses Playwright with C# and NUnit to automate high-value UI flows with strict assertions, Page Object Model design, and cross-browser execution on BrowserStack.

## Stack
- .NET 8 + C#
- Playwright 1.58.0
- NUnit 4.3.2
- BrowserStack Automate (CDP)

## Project Structure
```
Testathon.Tests/
├─ Framework/
│  └─ TestBase.cs          # Base test class with BrowserStack integration
├─ Pages/
│  ├─ BasePage.cs          # Base page object with common methods
│  ├─ HomePage.cs          # Home page object (products, filters, cart)
│  ├─ SignInPage.cs        # Sign-in page object (authentication)
│  └─ CheckoutPage.cs      # Checkout page object (order submission)
└─ Tests/
   ├─ Smoke/               # 12 tests (8 core + 4 vendor filters)
   ├─ Negative/            # 9 tests (7 core + 2 checkout)
   └─ Regression/          # 5 tests (4 core + 1 checkout)
```

## Test Coverage
- **26 automated tests** covering smoke, negative, and regression scenarios
- **Page Object Model** architecture for maintainability
- **Strict assertions** that expose real bugs (4 bugs found)
- **Authentication support** for protected pages
- **Vendor filtering validation** (Apple, Samsung, Google, OnePlus)
- **Checkout flow testing** with smart conditional execution
- **Cross-browser ready** via BrowserStack

## Bugs Found
- **3 Routing Bugs**: Offers/Orders/Favourites redirect to signin after authentication (HIGH)
- **1 Vendor Filter Bug**: OnePlus filter shows Apple products (HIGH)

## Setup

### Prerequisites
1. .NET 8 SDK installed
2. BrowserStack account (optional, for cloud execution)

### Install Playwright Browsers (for local execution)
```bash
cd Testathon.Tests/bin/Debug/net8.0
powershell -File playwright.ps1 install chromium
```

### Configure BrowserStack (optional)
1. Set environment variables:
   ```bash
   # Windows
   set BROWSERSTACK_USERNAME=your_username
   set BROWSERSTACK_ACCESS_KEY=your_access_key
   
   # Or create .env file (not tracked in git)
   ```
2. Tests automatically detect credentials and switch to BrowserStack execution

## Running Tests

### Run All Tests
```bash
dotnet test
```

### Run by Category
```bash
# Smoke tests only
dotnet test --filter "Category=Smoke"

# Negative tests only
dotnet test --filter "Category=Negative"

# Regression tests only
dotnet test --filter "Category=Regression"
```

### Run Specific Test
```bash
dotnet test --filter "FullyQualifiedName~TC_SM_01"
```

## Execution Modes

### Local Execution
- Runs when BrowserStack credentials are NOT set
- Uses local Chromium browser (Headless = false for visibility)
- Faster for development and debugging

### BrowserStack Execution
- Runs when BROWSERSTACK_USERNAME and BROWSERSTACK_ACCESS_KEY are set
- Executes on Chrome/Windows 11 in BrowserStack cloud
- Test names and build info automatically sent to dashboard

## Test Authentication
- Tests requiring login (Offers, Orders, Favourites) use demo credentials:
  - **Username**: demouser
  - **Password**: testingisfun99
- Authentication handled automatically by SignInPage

## Documentation
- **TEST_CASE_DOCUMENTATION.md**: Complete test case specifications with steps and pass criteria
- **ASSERTION_STRENGTHENING_REPORT.md**: Details on assertion improvements and bugs exposed
- **NEW_TESTS_IMPLEMENTATION_SUMMARY.md**: Implementation summary for vendor and checkout tests

## Test Results
- **Total**: 26 tests
- **Passing**: 20 tests
- **Failing**: 4 tests (exposing real bugs)
- **Skipped**: 2 tests (conditional execution when auth required)

## Links
- Test Site: https://testathon.live/
- BrowserStack Testathon: https://www.browserstack.com/testathon

## Notes
- All tests use async/await for Playwright operations
- Page Objects contain locators and actions (no assertions)
- Tests contain assertions and test flow logic
- No hard-coded credentials or secrets in code
- Failing tests indicate real bugs, not false positives
