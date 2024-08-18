using KalanalyzeCode.ConfigurationManager.Ui.AcceptanceTesting.Model;
using Microsoft.Playwright;

namespace KalanalyzeCode.ConfigurationManager.Ui.AcceptanceTesting.PageObjects;

public sealed class LoginPageObject : BasePageTemplateObject
{
    public override string PagePath => "Account/Login";

    public LoginPageObject(IBrowserContext browser, IPage page)
    {
        Page = page;
        Browser = browser;
    }
    
    public override IPage Page { get; set; }
    public override IBrowserContext Browser { get; }

    public ILocator Username => Page.GetByTestId("Input.Username");
    public ILocator Password => Page.GetByTestId("Input.Password");
    public ILocator SignInButton => Page.GetByTestId("Input.Login");
    public ILocator CancelButton => Page.GetByTestId("Input.Cancel");

    public async Task SetUsername(string email) => await Username.FillAsync(email);
    public async Task SetPassword(string password) => await Password.FillAsync(password);

    public async Task SetLoginData(LoginModel model)
    {
        await SetUsername(model.Email);
        await SetPassword(model.Password);
    }

    public async Task SignInButtonClicked() => await SignInButton.ClickAsync();
}