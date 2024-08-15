using Humanizer;

namespace KalanalyzeCode.ConfigurationManager.Ui.Helpers;

public sealed class AppConstant
{
    public const string Loading = "Loading...";
    public sealed class Projects
    {
        public sealed class Index
        {
            public const string Route = $"{nameof(Projects)}/{nameof(Index)}";
            public const string AppTitle = $"{nameof(Projects)} - {nameof(Index)}";
            public static readonly string Name = $"{nameof(Projects).Humanize()}";
        }
    }
    
    public sealed class ProjectManager
    {
        public class Index
        {
            public const string Route = $"{nameof(ProjectManager)}/{nameof(Index)}";
            public static readonly string AppTitle = $"{nameof(ProjectManager).Humanize()}";
        }
    }
}