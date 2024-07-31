namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;

[CollectionDefinition(Collections.ApiWebApplicationCollection)]
public class SharedTestCollection : ICollectionFixture<ApiWebApplication>
{
    
}

public class Collections
{
    public const string ApiWebApplicationCollection = "Test collection";
}