namespace KalanalyzeCode.ConfigurationManager.Ui.Helpers;

public static class Extension
{
    public static UpdateProjectRequest MapToUpdateProjectRequest(this Project source)
    {
        return new UpdateProjectRequest()
        {
            Id = source.Id,
            ProjectName = source.Name
        };
    }
}