using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Ui.Validators;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KalanalyzeCode.ConfigurationManager.Ui.Components.Pages.Projects.Components;

public partial class Add
{
    #region DependencyInjection

    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    
    #endregion

    #region Fields

    private readonly CreateProjectRequestValidator _validator = new();
    private MudForm _form = default!;

    #endregion

    #region Properties

    [Parameter, EditorRequired] public EventCallback<bool> Success { get; set; }
    private CreateProjectRequest Model { get; set; } = new();

    #endregion
    

    private async Task Submit()
    {
        await _form.Validate();

        if (_form.IsValid)
        {
            var result = await Mediator.Send(Model, CancellationToken);
            var project = result.Project;
            if (project is not null)
            {
                Snackbar.Add("Submitted");
                await Success.InvokeAsync(true);
                return;
            }

            Snackbar.Add("Failed", Severity.Error);
            await Success.InvokeAsync(false);
        }
    }
}