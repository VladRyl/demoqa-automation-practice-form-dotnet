using DemoQA.Tests.Config;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using Allure.Xunit;
using Allure.Net.Commons;

namespace DemoQA.Tests.Tests.Base;

public abstract class BaseTest : PageTest
{
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            BaseURL = TestConfig.Instance.BaseUrl,
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
            IgnoreHTTPSErrors = true
        };
    }

    public override async Task DisposeAsync()
    {
        if (Page != null)
        {
            var screenshot = await Page.ScreenshotAsync(new PageScreenshotOptions
            {
                FullPage = true
            });

            // Без await:
            AllureApi.AddAttachment(
                "Failure Screenshot",
                "image/png",
                screenshot,
                ".png"
            );
        }

        await base.DisposeAsync();
    }
}