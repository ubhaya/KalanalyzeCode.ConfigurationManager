using Microsoft.AspNetCore.Components;

namespace KalanalyzeCode.ConfigurationManager.Ui.Components.Pages.ProjectManager;

public partial class Index
{
    #region DependencyInjection

    [Inject] private IProjectManagerClient ProjectsClient { get; set; } = default!;
    [Inject] private IApiKeyManagerClient ApiKeyManagerClient { get; set; } = default!;
    
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
    
    private async Task OnApiKeyAddClick()
    {
        if (_project is null)
            return;
        _project.ApiKey = await ApiKeyManagerClient.PostAsync(new CreateApiKeyForProjectRequest
        {
            ProjectId = _project?.Id ?? Guid.Empty
        });
    }

    private async Task OnApiKeyDelete()
    {
        await ApiKeyManagerClient.DeleteAsync(_project?.Id ?? Guid.Empty);
        var response = await ProjectsClient.GetByIdAsync(Id, CancellationToken);
        _project = response.Project;
    }
}