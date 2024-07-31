using NetEscapades.EnumGenerators;

namespace Identity.Shared.Authorization;

[Flags]
[EnumExtensions]
public enum Permissions
{
    None = 0,
    Read = 1,
    Write = 2,
    AppSettings = 4,
    Project = 8,
    All = ~None,
}