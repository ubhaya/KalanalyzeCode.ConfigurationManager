using System.Net.Http.Json;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.ApiKeyManager;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using LanguageExt.Common;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.ApiKeyManager.Command;

public sealed class CreateApiKeyForProjectRequestHandler : IRequestHandler<CreateApiKeyForProjectRequest, Result<Guid>>
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IApplicationDbContext _context;

    public CreateApiKeyForProjectRequestHandler(IHttpClientFactory clientFactory, IApplicationDbContext context)
    {
        _clientFactory = clientFactory;
        _context = context;
    }

    public async Task<Result<Guid>> Handle(CreateApiKeyForProjectRequest request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects.FindAsync([request.ProjectId], cancellationToken);
        
        if (project is null)
            return new Result<Guid>(new Exception($"Project {request.ProjectId} not found"));

        if (project.ApiKey != Guid.Empty)
        {
            return project.ApiKey;
        }
        
        var client = _clientFactory.CreateClient(AppConstants.IdentityServerClient);

        var newApiKey = Guid.NewGuid();

        var result = await client.PostAsJsonAsync("api/Account", new RegisterRequest()
        {
            ApiKey = newApiKey.ToString("N")
        }, cancellationToken: cancellationToken);

        result.EnsureSuccessStatusCode();


        project.ApiKey = newApiKey;

        _context.Projects.Update(project);

        await _context.SaveChangesAsync(cancellationToken);
        return newApiKey;
    }
    
    private class RegisterRequest
    {
        public string ApiKey { get; set; } = string.Empty;
    }
}