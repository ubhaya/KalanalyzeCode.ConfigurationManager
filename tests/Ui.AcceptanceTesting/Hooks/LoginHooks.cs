using BoDi;
using KalanalyzeCode.ConfigurationManager.Ui.AcceptanceTesting.PageObjects;
using Microsoft.Playwright;
using TechTalk.SpecFlow;

namespace KalanalyzeCode.ConfigurationManager.Ui.AcceptanceTesting.Hooks;

[Binding]
public sealed class LoginHooks
{
    [BeforeScenario("Login")]
    public async Task BeforeLoginSuccessScenario(IObjectContainer container)
    {
        var playwright = await Playwright.CreateAsync();

        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
#if DEBUG
            Headless = false,
            SlowMo = 1000,
#endif
        });

        var page = await browser.NewPageAsync();
        var pageObject = new LoginPageObject(browser, page);
        
        container.RegisterInstanceAs(playwright);
        container.RegisterInstanceAs(browser);
        container.RegisterInstanceAs(page);
        container.RegisterInstanceAs(pageObject);
    }
    [AfterScenario]
    public async Task AfterScenarioAsync(IObjectContainer container)
    {
        var browser = container.Resolve<IBrowser>();
        await browser.CloseAsync();
        var playwright = container.Resolve<IPlaywright>();
        playwright.Dispose();
    }
}