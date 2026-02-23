# BrowserStack Configuration Guide

## Overview
This test suite is configured to run on BrowserStack using Playwright's CDP (Chrome DevTools Protocol) connection.

## Prerequisites
1. BrowserStack account with Playwright/CDP access enabled
2. Valid BrowserStack username and access key

## Environment Variables
Set the following environment variables to enable BrowserStack execution:

### Windows (PowerShell)
```powershell
$env:BROWSERSTACK_USERNAME = "your_username"
$env:BROWSERSTACK_ACCESS_KEY = "your_access_key"
$env:BROWSERSTACK_DEVICE = "chrome-win11"  # Optional, defaults to chrome-win11
```

### Windows (CMD)
```cmd
set BROWSERSTACK_USERNAME=your_username
set BROWSERSTACK_ACCESS_KEY=your_access_key
set BROWSERSTACK_DEVICE=chrome-win11
```

### Persistent (Windows)
```cmd
setx BROWSERSTACK_USERNAME "your_username"
setx BROWSERSTACK_ACCESS_KEY "your_access_key"
```

## Supported Devices
The framework supports the following device configurations via `BROWSERSTACK_DEVICE`:

- `chrome-win11` - Chrome on Windows 11 (default)
- `chrome-mac` - Chrome on macOS Sonoma
- `edge-win11` - Edge on Windows 11

## Running Tests on BrowserStack

### Single Device
```bash
# Set credentials
set BROWSERSTACK_USERNAME=your_username
set BROWSERSTACK_ACCESS_KEY=your_access_key
set BROWSERSTACK_DEVICE=chrome-win11

# Run tests
dotnet test
```

### Multiple Devices
Use the provided PowerShell script:
```powershell
.\run-browserstack-tests.ps1
```

This script will:
1. Set BrowserStack credentials
2. Run full test suite on Chrome/Windows 11
3. Run full test suite on Chrome/macOS
4. Run full test suite on Edge/Windows 11

## Local Execution
If BrowserStack credentials are not set, tests automatically fall back to local Chromium execution:
```bash
dotnet test
```

## Troubleshooting

### CDP Connection Issues
If you see "Protocol error (Browser.getVersion): undefined" or connection failures:

1. **Verify Playwright/CDP is enabled**: Contact BrowserStack support to ensure your account has Playwright CDP access enabled
2. **Check credentials**: Verify username and access key are correct
3. **Test connection**: Try connecting via BrowserStack's test page
4. **Fallback to local**: Unset environment variables to run locally

### Invalid Username or Password
```
Error: Invalid username or password
```
- Double-check your BrowserStack username and access key
- Ensure no extra spaces in credentials
- Verify account is active

### WebSocket Disconnection
```
<ws disconnected> wss://cdp.browserstack.com/playwright code=1005
```
- This may indicate CDP/Playwright is not enabled for your account
- Contact BrowserStack support to enable Playwright access
- Use local execution as fallback

## BrowserStack Dashboard
View test results at: https://automate.browserstack.com/dashboard

## Notes
- Tests automatically send test names and build info to BrowserStack
- Build name: "testathon-playwright-csharp"
- Each test appears as a separate session in the dashboard
- Local execution uses Chromium with Headless=false for visibility

## Account Setup
For BrowserStack Testathon participants:
1. Sign up at https://www.browserstack.com/testathon
2. Get your credentials from Account > Settings
3. Ensure Playwright/CDP access is enabled (contact support if needed)
4. Set environment variables as shown above
