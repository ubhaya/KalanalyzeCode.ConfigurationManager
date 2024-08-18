using Ardalis.GuardClauses;
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

        var context = await playwright.Chromium.LaunchPersistentContextAsync("", new BrowserTypeLaunchPersistentContextOptions
        {
            IgnoreHTTPSErrors = true,
#if DEBUG
            Headless = false,
            SlowMo = 1000,
#endif
        });
        
        var pageObject = new LoginPageObject(context, context.Pages[0]);
        
        container.RegisterInstanceAs(playwright);
        container.RegisterInstanceAs(context);
        container.RegisterInstanceAs(context.Pages[0]);
        container.RegisterInstanceAs(pageObject);
    }
    [AfterScenario]
    public async Task AfterScenarioAsync(IObjectContainer container)
    {
        var browser = container.Resolve<IBrowserContext>();
        await browser.CloseAsync();
        var playwright = container.Resolve<IPlaywright>();
        playwright.Dispose();
    }
}