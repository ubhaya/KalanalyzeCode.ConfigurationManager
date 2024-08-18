using Microsoft.Playwright;

namespace KalanalyzeCode.ConfigurationManager.Ui.AcceptanceTesting.PageObjects;

public abstract class BasePageObject
{
    private readonly string _basePath = ConfigurationHelper.GetBaseUrl();
    
    public abstract string PagePath { get; }
    public abstract IPage Page { get; set; }
    public abstract IBrowserContext Browser { get; }
    
    public async Task NavigateToAsync()
    {
        await Page.GotoAsync(_basePath+PagePath);
    }
    
    public async Task NavigateToHomePageAsync()
    {
        await Page.GotoAsync(_basePath);
    }
}