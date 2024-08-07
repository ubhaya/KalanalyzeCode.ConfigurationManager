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
}