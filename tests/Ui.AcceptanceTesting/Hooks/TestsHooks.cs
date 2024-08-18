using TechTalk.SpecFlow;

namespace KalanalyzeCode.ConfigurationManager.Ui.AcceptanceTesting.Hooks;

[Binding]
public sealed class TestsHooks
{
    private static readonly ConfigurationManagerWebApplication App = new();
    [BeforeTestRun]
    public static async Task BeforeAnyScenario()
    {
        await App.InitializeAsync();
        App.CreateDefaultClient();
        await App.InitializeRespawner();
    }

    [AfterTestRun]
    public static async Task AfterAnyScenario()
    {
        await App.ResetDatabaseAsync();
        await App.DisposeAsync();
    }
}