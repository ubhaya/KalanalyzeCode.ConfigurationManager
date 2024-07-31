using NetEscapades.EnumGenerators;

namespace Identity.Shared.Authorization;

[Flags]
[EnumExtensions]
public enum Permissions
{
    None = 0,
    GetAppSettings = 1,
    ProjectRead = 2,
    All = ~None,
}