namespace KalanalyzeCode.ConfigurationManager.Ui.Authorization;

public class PermissionsProvider
{
    public static List<Permissions> GetAll()
    {
        return PermissionsExtensions.GetValues().ToList();
    }
}
