using KalanalyzeCode.ConfigurationManager.Application.Contract.Request;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Configurations;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Ui.Validators;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KalanalyzeCode.ConfigurationManager.Ui.Components.Pages.ProjectManager;

public partial class Add
{
    #region DependencyInjection

    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = default!;
    
    #endregion

    #region Fields

    private readonly CreateConfigurationRequestValidator _validator = new();
    private MudForm _form = default!;

    #endregion

    #region Properties

    private CreateConfigurationRequest Model { get; set; } = new();
    [Parameter] public Guid ProjectId { get; set; }

    #endregion

    protected override void OnParametersSet()
    {
        Model.ProjectId = ProjectId;
    }

    private async Task Submit()
    {
        await _form.Validate();

        if (_form.IsValid)
        {
            var result = await Mediator.Send(Model, CancellationToken);
            var project = result.Configuration;
            if (project is not null)
            {
                Snackbar.Add("Submitted");
                MudDialog.Close(project);
                return;
            }

            Snackbar.Add("Failed", Severity.Error);
            MudDialog.Cancel();
        }
    }
}