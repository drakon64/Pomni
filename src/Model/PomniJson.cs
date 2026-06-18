using System.Text.Json.Serialization;

namespace pomni.Model;

internal class PomniJson
{
    public required Forge Forge { get; init; }
    public required string Repository { get; init; }
    public required string Revision { get; init; }
    public required ReferenceType ReferenceType { get; init; }

    public string? Reference { get; init; } = null;
    public bool? Frozen { get; init; } = false;
}

internal enum Forge
{
    [JsonStringEnumMemberName("github")]
    GitHub,
}

internal enum ReferenceType
{
    [JsonStringEnumMemberName("commit")]
    Commit,

    Release,
    Tag,
}
