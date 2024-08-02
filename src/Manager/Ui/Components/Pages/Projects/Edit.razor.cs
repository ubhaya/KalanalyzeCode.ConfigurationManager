using Ardalis.GuardClauses;
using KalanalyzeCode.ConfigurationManager.Ui.Validators;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KalanalyzeCode.ConfigurationManager.Ui.Components.Pages.Projects;

public partial class Edit
{
    #region DependencyInjection

    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] private IProjectsClient Client { get; set; } = default!;

    #endregion

    #region Fields

    private readonly UpdateProjectRequestValidator _validator = new();
    private MudForm _form = default!;

    #endregion

    #region Properties

    [Parameter, EditorRequired] public EventCallback<bool> Success { get; set; }
    [Parameter, EditorRequired] public UpdateProjectRequest? Model { get; set; }

    #endregion


    private async Task Submit()
    {
        Guard.Against.Null(Model);
        await _form.Validate();

        if (_form.IsValid)
        {
            await Client.PutAsync(Model.Id, Model, CancellationToken);
            Snackbar.Add("Submitted");
            await Success.InvokeAsync(true);
        }
    }
}