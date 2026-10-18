using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using DemoQA.Tests.Config;

namespace DemoQA.Tests.Tests.Base;

public abstract class BaseTest : PageTest
{
    static BaseTest()
    {
        Environment.SetEnvironmentVariable("HEADED", "1");
    }

    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            BaseURL = TestConfig.Instance.BaseUrl,
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
            IgnoreHTTPSErrors = true
        };
    }
}