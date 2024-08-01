using KalanalyzeCode.ConfigurationManager.Ui.Validators;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KalanalyzeCode.ConfigurationManager.Ui.Components.Pages.Projects;

public partial class Add
{
    #region DependencyInjection

    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] private IProjectsClient Client { get; set; } = default!;
    
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
            var result = await Client.PostAsync(Model, CancellationToken);
            if (result is not null && result.Success)
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