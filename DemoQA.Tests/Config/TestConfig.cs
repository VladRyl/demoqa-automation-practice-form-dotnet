using Microsoft.Extensions.Configuration;

namespace DemoQA.Tests.Config;

public class TestConfig
{
    public string BaseUrl { get; set; } = string.Empty;

    private static TestConfig? _instance;
    public static TestConfig Instance => _instance ??= Load();

    private static TestConfig Load()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var settings = new TestConfig();
        config.GetSection("TestSettings").Bind(settings);
        return settings;
    }
}