using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;

namespace KalanalyzeCode.ConfigurationManager.Ui.Extension;

public static class Extension
{
    public static UpdateProjectRequest MapToUpdateProjectRequest(this Project source)
    {
        return new UpdateProjectRequest
        {
            Id = source.Id,
            ProjectName = source.Name
        };
    }
}