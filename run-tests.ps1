param(
    [string]$Filter = "",
    [switch]$NoOpen
)

$ErrorActionPreference = "Stop"
Set-Location $PSScriptRoot

$results = Join-Path $PSScriptRoot "bin\Debug\net8.0\allure-results"
$report  = Join-Path $PSScriptRoot "allure-report"

# Keep history so the report shows trends, then clear old results
$history = Join-Path $report "history"
$savedHistory = Join-Path $env:TEMP "allure-history-$PID"
if (Test-Path $history) { Copy-Item $history $savedHistory -Recurse -Force }
Remove-Item $results -Recurse -Force -ErrorAction SilentlyContinue

if ($Filter) { dotnet test --filter $Filter } else { dotnet test }
$exit = $LASTEXITCODE

if (Test-Path $savedHistory) {
    Copy-Item $savedHistory (Join-Path $results "history") -Recurse -Force
    Remove-Item $savedHistory -Recurse -Force
}

if (-not (Get-Command allure -ErrorAction SilentlyContinue)) {
    Write-Warning "Allure CLI not found. Install it with: npm install -g allure-commandline (requires Java)"
    exit $exit
}

if (-not $env:JAVA_HOME -and -not (Get-Command java -ErrorAction SilentlyContinue)) {
    Write-Warning "Java not found - Allure CLI needs it. Install with: winget install EclipseAdoptium.Temurin.17.JRE (then reopen the terminal)"
    exit $exit
}

# Generate the report even when tests fail
allure generate $results -o $report --clean
if ($LASTEXITCODE -ne 0) { Write-Warning "allure generate failed"; exit $exit }
Write-Host "Allure report: $report\index.html"
if (-not $NoOpen) { allure open $report }

exit $exit
