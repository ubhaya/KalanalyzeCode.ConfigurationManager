using NetEscapades.EnumGenerators;

namespace Identity.Shared.Authorization;

[Flags]
[EnumExtensions]
public enum Permissions
{
    None = 0,
    All = ~None
}