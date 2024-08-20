using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Configurations;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Configurations;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.Configurations.Commands;

public sealed class CreateConfigurationRequestHandler : IRequestHandler<CreateConfigurationRequest, CreateConfigurationResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateConfigurationRequestHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CreateConfigurationResponse> Handle(CreateConfigurationRequest request, CancellationToken cancellationToken)
    {
        var configuration = new Configuration
        {
            Name = request.Name,
            Value = request.Value,
            ProjectId = request.ProjectId,
        };
        
        _context.Configurations.Add(configuration);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return new CreateConfigurationResponse(configuration);
    }
}