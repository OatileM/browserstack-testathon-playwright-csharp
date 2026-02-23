# BrowserStack Multi-Device Test Execution Script
# Runs test suite across multiple browser/OS combinations

$env:BROWSERSTACK_USERNAME = "oatilemdlela_4X1Ehc"
$env:BROWSERSTACK_ACCESS_KEY = "QbjiV21S6QirbuYYBagN"

$devices = @(
    "chrome-win11",
    "chrome-mac",
    "edge-win11"
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "BrowserStack Multi-Device Test Execution" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

foreach ($device in $devices) {
    Write-Host "Running tests on: $device" -ForegroundColor Yellow
    Write-Host "----------------------------------------" -ForegroundColor Yellow
    
    $env:BROWSERSTACK_DEVICE = $device
    
    dotnet test --logger "console;verbosity=normal"
    
    Write-Host ""
    Write-Host "Completed: $device" -ForegroundColor Green
    Write-Host ""
}

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "All device tests completed!" -ForegroundColor Cyan
Write-Host "Check BrowserStack dashboard for results" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
