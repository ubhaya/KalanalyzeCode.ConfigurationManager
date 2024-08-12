using Microsoft.AspNetCore.Components;

namespace KalanalyzeCode.ConfigurationManager.Ui.Components.Pages.ProjectManager;

public partial class Index
{
    #region DependencyInjection

    [Inject] private IProjectsClient ProjectsClient { get; set; } = default!;
    
    #endregion

    #region Fields

    private Project? _project;

    #endregion

    #region Properties

    [Parameter] public Guid Id { get; set; }

    #endregion

    protected override async Task OnInitializedAsync()
    {
        var response = await ProjectsClient.GetByIdAsync(Id, CancellationToken);
        _project = response.Project;
    }
}