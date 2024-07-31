using Microsoft.AspNetCore.Components;

namespace KalanalyzeCode.ConfigurationManager.Ui.Components.Pages.Projects;

public partial class Index
{
    private ICollection<Project>? _projects;
    [Inject] private IProjectsClient Client { get; set; } = default!;
    
    //private 

    protected override async Task OnInitializedAsync()
    {
        var result = await Client.GetAllAsync();

        _projects = result?.Data.Projects;
    }
}