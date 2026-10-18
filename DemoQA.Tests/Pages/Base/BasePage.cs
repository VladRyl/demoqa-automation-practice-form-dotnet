using Microsoft.Playwright;
using DemoQA.Tests.Config;

namespace DemoQA.Tests.Pages.Base;

public abstract class BasePage
{
    protected readonly IPage Page;
    protected readonly string BaseUrl;

    protected BasePage(IPage page)
    {
        Page = page;
        BaseUrl = TestConfig.Instance.BaseUrl;
    }

    public async Task NavigateToAsync(string relativeUrl = "")
    {
        var targetUrl = string.IsNullOrEmpty(relativeUrl) 
            ? BaseUrl 
            : $"{BaseUrl.TrimEnd('/')}/{relativeUrl.TrimStart('/')}";

        await Page.GotoAsync(targetUrl);
    }

    public async Task<string> GetTitleAsync()
    {
        return await Page.TitleAsync();
    }
}