using Ardalis.GuardClauses;
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
    private Configuration? _configurationBeforeEditing;
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

    private async Task<TableData<Configuration>> ServerReload(TableState state, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetAllConfigurationRequest(
            _searchString, state.Page, state.PageSize,
            (CustomSortDirection)state.SortDirection, state.SortLabel ?? string.Empty,
            _project!.Id), CancellationToken);

        return new TableData<Configuration>()
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

    private async Task OnItemDeleted(Configuration? configuration)
    {
        Guard.Against.Null(configuration);
        var request = new DeleteConfigurationRequest(configuration.Id);
        await Mediator.Send(request, CancellationToken);
    }

    private void RowEditPreview(object? obj)
    {
        var configuration = obj as Configuration;
        Guard.Against.Null(configuration);
        _configurationBeforeEditing = new Configuration
        {
            Id = configuration.Id,
            Name = configuration.Name,
            Value = configuration.Value,
            ProjectId = configuration.ProjectId
        };
    }

    private void ResetItemToOriginalValues(object? configuration)
    {
        Guard.Against.Null(configuration);
        Guard.Against.Null(_configurationBeforeEditing);
        ((Configuration)configuration).Id = _configurationBeforeEditing.Id;
        ((Configuration)configuration).Name = _configurationBeforeEditing.Name;
        ((Configuration)configuration).Value = _configurationBeforeEditing.Value;
        ((Configuration)configuration).ProjectId = _configurationBeforeEditing.ProjectId;
    }

    private async Task CommitEdit(object? configurationAsObject)
    {
        var configuration = configurationAsObject as Configuration;
        Guard.Against.Null(configuration);
        var request = new UpdateConfigurationRequest(configuration.Id, configuration.Name, configuration.Value);
        await Mediator.Send(request, CancellationToken);
    }
}