using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;
using KalanalyzeCode.ConfigurationManager.Ui.Components.Pages.Projects.Components;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KalanalyzeCode.ConfigurationManager.Ui.Components.Pages.Projects;

public partial class Index
{
    #region DependencyInjection

    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private IDialogService DialogService { get; set; } = default!;

    #endregion

    #region Fields

    private readonly DialogOptions _dialogOptions = new()
    {
        CloseButton = true
    };

    private MudTable<Project> _projectTable = default!;
    private string _searchString = string.Empty;

    #endregion

    #region Properties



    #endregion

    private async Task<TableData<Project>> ServerReload(TableState state, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetAllProjectsRequest(
            _searchString, state.Page, state.PageSize,
            (CustomSortDirection)state.SortDirection, state.SortLabel ?? string.Empty
        ), cancellationToken);
        return new TableData<Project>
        {
            Items = result.Projects,
            TotalItems = result.TotalItem
        };
    }

    private async Task OnSearch(string text)
    {
        _searchString = text;
        await _projectTable.ReloadServerData();
    }

    private async Task Add()
    {
        var dialog = await DialogService.ShowAsync<AddEditModel>("Add project", _dialogOptions);
        var result = await dialog.Result;
        if ((!result?.Canceled) ?? false)
        {
            await _projectTable.ReloadServerData();
        }
    }

    private async Task Edit(Project project)
    {
        var parameters = new DialogParameters<AddEditModel>
        {
            { x => x.EditModel, project },
        };
        var dialog = await DialogService.ShowAsync<AddEditModel>("Edit project", parameters, _dialogOptions);
        var result = await dialog.Result;
        if ((!result?.Canceled) ?? false)
        {
            await _projectTable.ReloadServerData();
        }
    }

    private async Task Delete(Project project)
    {
        var parameters = new DialogParameters<DeleteModel>
        {
            { x => x.Model, project },
        };
        var dialog = await DialogService.ShowAsync<DeleteModel>("Delete ship", parameters, _dialogOptions);
        var result = await dialog.Result;
        if ((!result?.Canceled) ?? false)
        {
            await _projectTable.ReloadServerData();
        }
    }
}