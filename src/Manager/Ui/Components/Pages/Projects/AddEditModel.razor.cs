using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KalanalyzeCode.ConfigurationManager.Ui.Components.Pages.Projects;

public partial class AddEditModel
{
    #region DependencyInjection

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = default!;

    #endregion

    #region Fields

    #endregion

    #region Properties

    [Parameter] public Project? EditModel { get; set; }

    #endregion

    private void Submit(bool status)
    {
        if (status)
        {
            MudDialog.Close();
            return;
        }

        MudDialog.Cancel();
    }
}