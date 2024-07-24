using KalanalyzeCode.ConfigurationManager.Application.Common.Interfaces;
using KalanalyzeCode.ConfigurationManager.Shared;
using Microsoft.EntityFrameworkCore;

namespace KalanalyzeCode.ConfigurationManager.Application;

public class RepositoryService
{
    private readonly IApplicationDbContext _context;

    public RepositoryService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ApplicationSettings>> GetAllApplicationSettings(string settingName, CancellationToken cancellationToken = default)
    {
        return await _context.Settings.Where(l => l.Id.StartsWith(settingName))
            .Select(s => new ApplicationSettings(s.Id, s.Value))
            .ToListAsync(cancellationToken);
    }
}

