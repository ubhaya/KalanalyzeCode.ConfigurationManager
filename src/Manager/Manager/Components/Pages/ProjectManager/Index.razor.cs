using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.ApiKeyManager;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace KalanalyzeCode.ConfigurationManager.Ui.Components.Pages.ProjectManager;

public partial class Index
{
    #region DependencyInjection

    [Inject] private IMediator Mediator { get; set; } = default!;
    
    #endregion

    #region Fields

    private Project? _project;

    #endregion

    #region Properties

    [Parameter] public Guid Id { get; set; }

    #endregion

    protected override async Task OnInitializedAsync()
    {
        var response = await Mediator.Send(new GetProjectByIdRequest(Id), CancellationToken);
        _project = response.Project;
    }
    
    private async Task OnApiKeyAddClick()
    {
        if (_project is null)
            return;
        var result = await Mediator.Send(new CreateApiKeyForProjectRequest(_project?.Id ?? Guid.Empty));
        result.IfSucc(apiKey => _project!.ApiKey = apiKey);
    }

    private async Task OnApiKeyDelete()
    {
        await Mediator.Send(new DeleteApiKeyForProjectRequest(_project?.Id ?? Guid.Empty), CancellationToken);
        var response = await Mediator.Send(new GetProjectByIdRequest(Id), CancellationToken);
        _project = response.Project;
    }
}