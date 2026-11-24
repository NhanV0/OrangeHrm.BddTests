# OrangeHRM BDD UI Test Automation

Playwright (.NET) + Reqnroll (Gherkin) + NUnit + Allure Report.
Test design document: [docs/01.TestDesign.txt](docs/01.TestDesign.txt)
Framework design document: [docs/02.AutomationFrameworkDesign.txt](docs/02.AutomationFrameworkDesign.txt)

## Prerequisites
* .NET SDK 8.0+
* PowerShell (`pwsh`): needed once to install Playwright browsers
* Allure CLI (`scoop install allure` / `brew install allure` / `npm i -g allure-commandline`)

## Setup
```bash
dotnet restore
dotnet build
pwsh bin/Debug/net8.0/playwright.ps1 install --with-deps chromium
```

## Run
```bash
dotnet test                                   # all tests, headed (see appsettings.json)
dotnet test --filter "Category=smoke"         # only @smoke
dotnet test --filter "Category=negative"      # only @negative
Test__Headless=true dotnet test               # headless (CI)   | PowerShell: $env:Test__Headless="true"
```

## Allure report
```bash
allure serve bin/Debug/net8.0/allure-results
# or generate static html
allure generate bin/Debug/net8.0/allure-results -o allure-report --clean
```

## Project layout
```
Features/          Gherkin scenarios (living test cases)
StepDefinitions/   Glue code: Gherkin step to page object calls + assertions
Pages/             Page Objects (locators + actions)
Hooks/             Browser lifecycle, screenshot on failure
Support/           Settings loader, per-scenario Playwright driver
docs/              Test design and framework design documents
```
