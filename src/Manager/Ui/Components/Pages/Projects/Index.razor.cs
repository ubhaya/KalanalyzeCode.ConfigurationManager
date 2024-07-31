using Microsoft.AspNetCore.Components;

namespace KalanalyzeCode.ConfigurationManager.Ui.Components.Pages.Projects;

public partial class Index
{
    [Inject] private IProjectsClient Client { get; set; } = default!;
    
    //private 

    protected override async Task OnInitializedAsync()
    {
        var result = await Client.GetAllAsync();
    }
}