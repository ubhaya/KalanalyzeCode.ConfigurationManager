using Ardalis.GuardClauses;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace KalanalyzeCode.ConfigurationManager.Ui.Components.Pages.Projects.Components;

public partial class DeleteModel
{
    #region DependencyInjection

    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [CascadingParameter] private MudDialogInstance DialogInstance { get; set; } = default!;

    #endregion

    #region Fields

    #endregion

    #region Properties

    [Parameter, EditorRequired] public Project? Model { get; set; }

    #endregion

    private void Cancel(MouseEventArgs obj)
    {
        DialogInstance.Cancel();
    }

    private async Task DeleteMotor(CancellationToken cancellationToken)
    {
        Guard.Against.Null(Model);
        await Mediator.Send(new DeleteProjectRequest(Model.Id), cancellationToken);
        Snackbar.Add("Project Deleted", Severity.Success);
        DialogInstance.Close(DialogResult.Ok(Model.Id));
    }
}