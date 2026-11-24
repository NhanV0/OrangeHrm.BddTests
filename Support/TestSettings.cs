using Microsoft.Extensions.Configuration;

namespace OrangeHrm.BddTests.Support;

/// <summary>Strongly-typed access to appsettings.json (env vars override, e.g. Test__Headless=true).</summary>
public sealed class TestSettings
{
    public string BaseUrl { get; init; } = "https://opensource-demo.orangehrmlive.com";
    public string Browser { get; init; } = "chromium";
    public bool Headless { get; init; }
    public float SlowMoMs { get; init; }
    public float DefaultTimeoutMs { get; init; } = 15000;
    public bool ScreenshotOnFailure { get; init; } = true;

    public Credentials AdminUser { get; init; } = new();

    public sealed class Credentials
    {
        public string Username { get; init; } = "";
        public string Password { get; init; } = "";
    }

    private static readonly Lazy<TestSettings> LazyInstance = new(Load);
    public static TestSettings Current => LazyInstance.Value;

    private static TestSettings Load()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        var section = config.GetSection("Test");
        var admin = config.GetSection("Users:Admin");

        return new TestSettings
        {
            BaseUrl = section["BaseUrl"] ?? "https://opensource-demo.orangehrmlive.com",
            Browser = section["Browser"] ?? "chromium",
            Headless = section.GetValue("Headless", false),
            SlowMoMs = section.GetValue("SlowMoMs", 0f),
            DefaultTimeoutMs = section.GetValue("DefaultTimeoutMs", 15000f),
            ScreenshotOnFailure = section.GetValue("ScreenshotOnFailure", true),
            AdminUser = new Credentials
            {
                Username = admin["Username"] ?? "",
                Password = admin["Password"] ?? ""
            }
        };
    }
}
