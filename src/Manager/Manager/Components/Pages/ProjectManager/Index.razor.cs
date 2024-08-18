using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.ApiKeyManager;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Configurations;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KalanalyzeCode.ConfigurationManager.Ui.Components.Pages.ProjectManager;

public partial class Index
{
    #region DependencyInjection

    [Inject] private IMediator Mediator { get; set; } = default!;
    
    #endregion

    #region Fields

    private Project? _project;
    private MudDataGrid<Configuration> _configurationDataGrid = default!;
    private string _searchString = string.Empty;

    #endregion

    #region Properties

    [Parameter] public Guid Id { get; set; }

    #endregion

    protected override async Task OnInitializedAsync()
    {
        var response = await Mediator.Send(new GetProjectByIdRequest(Id), CancellationToken);
        _project = response.Project;
    }
    
    private async Task OnApiKeyAddClick()
    {
        if (_project is null)
            return;
        var result = await Mediator.Send(new CreateApiKeyForProjectRequest(_project?.Id ?? Guid.Empty));
        result.IfSucc(apiKey => _project!.ApiKey = apiKey);
    }

    private async Task OnApiKeyDelete()
    {
        await Mediator.Send(new DeleteApiKeyForProjectRequest(_project?.Id ?? Guid.Empty), CancellationToken);
        var response = await Mediator.Send(new GetProjectByIdRequest(Id), CancellationToken);
        _project = response.Project;
    }

    private async Task<GridData<Configuration>> ServerReload(GridState<Configuration> state)
    {
        var sortDefinition = state.SortDefinitions.FirstOrDefault();
        var sortDirection = sortDefinition?.Descending ?? false
            ? CustomSortDirection.Descending
            : CustomSortDirection.Ascending;
        var result = await Mediator.Send(new GetAllConfigurationRequest(
            _searchString, state.Page, state.PageSize,
            sortDirection, sortDefinition?.SortBy ?? string.Empty,
            _project!.Id), CancellationToken);

        return new GridData<Configuration>()
        {
            Items = result.Configurations,
            TotalItems = result.TotalItems
        };
    }
    
    private async Task OnSearch(string text)
    {
        _searchString = text;
        await _configurationDataGrid.ReloadServerData();
    }

    private Task OnItemChanged(Configuration configuration)
    {
        Console.WriteLine(configuration.Value);
        return Task.CompletedTask;
    }
}