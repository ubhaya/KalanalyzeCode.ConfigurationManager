using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Ui.AcceptanceTesting.Model;
using KalanalyzeCode.ConfigurationManager.Ui.AcceptanceTesting.PageObjects;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace KalanalyzeCode.ConfigurationManager.Ui.AcceptanceTesting.Steps;

[Binding]
public class LoginSteps
{
    private readonly LoginPageObject _pageObject;

    public LoginSteps(LoginPageObject pageObject)
    {
        _pageObject = pageObject;
    }

    [Given(@"User that not log in")]
    public async Task GivenUserThatNotLogIn()
    {
        await _pageObject.NavigateToHomePageAsync();
        var isLoginButtonVisible = await _pageObject.IsLoginButtonVisible();
        var isLogoutButtonVisible = await _pageObject.IsLogoutButtonVisible();
        isLoginButtonVisible.Should().BeTrue();
        isLogoutButtonVisible.Should().BeFalse();
    }

    [Given(@"Navigate to login page")]
    public async Task GivenNavigateToLoginPage()
    {
        await _pageObject.LoginButtonClicked();
        _pageObject.Page.Url.Should().Contain("Account/Login");
    }

    [When(@"User input correct credential")]
    public async Task WhenUserInputCorrectCredential(Table table)
    {
        var loginModel = table.CreateSet<LoginModel>().First();
        await _pageObject.SetLoginData(loginModel);
        await _pageObject.SignInButtonClicked();
    }

    [Then(@"User should be able to login")]
    public async Task ThenUserShouldBeAbleToLogin()
    {
        var isLoginButtonVisible = await _pageObject.IsLoginButtonVisible();
        var isLogoutButtonVisible = await _pageObject.IsLogoutButtonVisible();
        isLoginButtonVisible.Should().BeFalse();
        isLogoutButtonVisible.Should().BeTrue();
    }

    [Given(@"User that logged in")]
    public async Task GivenUserThatLoggedIn()
    {
        await LoginAsync();
        var isLoginButtonVisible = await _pageObject.IsLoginButtonVisible();
        var isLogoutButtonVisible = await _pageObject.IsLogoutButtonVisible();
        isLoginButtonVisible.Should().BeFalse();
        isLogoutButtonVisible.Should().BeTrue();
    }

    private async Task LoginAsync()
    {
        await _pageObject.NavigateToHomePageAsync();
        await _pageObject.LoginButtonClicked();
        var loginModel = new LoginModel()
        {
            Email = "bob",
            Password = "Pass123$",
        };
        await _pageObject.SetLoginData(loginModel);
        await _pageObject.SignInButtonClicked();
    }

    [When(@"User tried to logged out")]
    public async Task WhenUserTriedToLoggedOut()
    {
        await _pageObject.LogoutButtonClicked();
    }

    [Then(@"User should be able to log out")]
    public void ThenUserShouldBeAbleToLogOut()
    {
        _pageObject.Page.Url.Should().Contain("Account/Logout/LoggedOut");
    }
}