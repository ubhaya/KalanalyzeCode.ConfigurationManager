using Microsoft.Playwright;

namespace KalanalyzeCode.ConfigurationManager.Ui.AcceptanceTesting.PageObjects;

public abstract class BasePageTemplateObject : BasePageObject
{
    public ILocator LoginButton => Page.GetByTestId("login");
    public ILocator LogoutButton => Page.GetByTestId("logout");
    public ILocator ProfileButton => Page.GetByTestId("profile");
    public ILocator HomeButton => Page.GetByTestId("home");
    public ILocator WeatherButton => Page.GetByTestId("weather");
    public ILocator ProjectsButton => Page.GetByTestId("projects");
    public ILocator ProjectIndexButton => Page.GetByTestId("projects-index");
    
    public async Task<bool> IsLoginButtonVisible() => await LoginButton.IsVisibleAsync();
    public async Task LoginButtonClicked() => await LoginButton.ClickAsync();
    
    public async Task<bool> IsLogoutButtonVisible() => await LogoutButton.IsVisibleAsync();
    public async Task<bool> IsProfileButtonVisible() => await ProfileButton.IsVisibleAsync();
    
    public async Task<bool> IsHomeButtonVisible() => await HomeButton.IsVisibleAsync();
    public async Task<bool> IsWeatherButtonVisible() => await WeatherButton.IsVisibleAsync();
    public async Task<bool> IsProjectsButtonVisible() => await ProjectsButton.IsVisibleAsync();
    public async Task<bool> IsProjectCreateButtonVisible() => await ProjectsButton.IsVisibleAsync();
}