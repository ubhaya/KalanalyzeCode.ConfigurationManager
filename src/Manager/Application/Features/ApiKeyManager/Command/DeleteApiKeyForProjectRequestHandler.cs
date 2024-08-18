using System.Net.Http.Json;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.ApiKeyManager;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.ApiKeyManager.Command;

public sealed class DeleteApiKeyForProjectRequestHandler : IRequestHandler<DeleteApiKeyForProjectRequest>
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IApplicationDbContext _context;

    public DeleteApiKeyForProjectRequestHandler(IHttpClientFactory clientFactory, IApplicationDbContext context)
    {
        _clientFactory = clientFactory;
        _context = context;
    }

    public async Task Handle(DeleteApiKeyForProjectRequest request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects.FindAsync([request.ProjectId], cancellationToken);

        if (project is null || project.ApiKey == Guid.Empty)
            return;

        var client = _clientFactory.CreateClient(AppConstants.IdentityServerClient);

        await DeleteApiKey(project.ApiKey, cancellationToken);
        
        project.ApiKey = Guid.Empty;

        _context.Projects.Update(project);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task DeleteApiKey(Guid id, CancellationToken cancellationToken = default)
    {
        var isApiKeyExists =
            await _context.Users.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);

        if (isApiKeyExists is null)
            return;

        _context.Users.Remove(isApiKeyExists);
        await _context.SaveChangesAsync(cancellationToken);
    }
}