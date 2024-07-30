﻿using System.Net.Http.Headers;
using System.Text.Json;
using KalanalyzeCode.ConfigurationManager.Provider.Client;
using KalanalyzeCode.ConfigurationManager.Provider.Options;
using Microsoft.Extensions.Configuration;

namespace KalanalyzeCode.ConfigurationManager.Provider;

public class ConfigurationManagerProvider : ConfigurationProvider, IDisposable
{
    private readonly Timer? _timer;
    private IAppSettingsClient? _client;
    private readonly HttpClient _identityClient;

    public ConfigurationManagerProvider(ConfigurationManagerSource source)
    {
        Source = source;

        source.OptionsAction(Options);

        if (Options.ReloadPeriodically)
        {
            _timer = new Timer(
                callback: ReloadSettings,
                dueTime: TimeSpan.FromSeconds(10),
                period: TimeSpan.FromSeconds(Options.PeriodInSeconds),
                state: null
            );
        }

        _identityClient = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:5001")
        };
    }

    private ConfigurationManagerSource Source { get; }
    private ConfigurationOptions Options { get; } = new();
    
    public override void Load()
    {
        var formData = new FormUrlEncodedContent([
            new KeyValuePair<string, string>("grant_type", "api_key"),
            new KeyValuePair<string, string>("scope", Options.SecreteManagerOptions.Scope),
            new KeyValuePair<string, string>("client_id", Options.SecreteManagerOptions.ClientId),
            new KeyValuePair<string, string>("client_secret", Options.SecreteManagerOptions.ClientSecrete),
            new KeyValuePair<string, string>("api_key", Options.SecreteManagerOptions.ApiKey),
        ]);
        var identityResult = Task.Run(async () => 
                await _identityClient.PostAsync("connect/token", formData))
            .Result;

        identityResult.EnsureSuccessStatusCode();

        var message = Task.Run(async()=>await identityResult.Content.ReadAsStringAsync()).Result;

        using var jsonDocument = JsonDocument.Parse(message);

        var root = jsonDocument.RootElement;

        var accessToken = root.GetProperty("access_token").GetString();

        _client ??= new AppSettingsClient(new HttpClient()
        {
            BaseAddress = Options.SecreteManagerOptions.BaseAddress,
            DefaultRequestHeaders =
            {
                Authorization = new AuthenticationHeaderValue("Bearer", accessToken)
            }
        });
        
        var result = Task.Run(async () => 
                await _client.GetAppSettingsRequestAsync("StarfishOptions"))
            .Result;

        Data = result?.Data?.Settings
                   .ToDictionary<ApplicationSettings, string, string?>(c => c.Id, c => c.Value,
                       StringComparer.OrdinalIgnoreCase) ??
               [];
    }

    private void ReloadSettings(object? state)
    {
        Load();
        OnReload();
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}