using System.Text.Json.Serialization;

namespace pomni.Model;

internal class PomniJson
{
    public required Forge Forge { get; init; }
    public required string Repository { get; init; }

    public string? Reference { get; init; }
    public ReferenceType? ReferenceType { get; init; }
    public bool? Frozen { get; init; }
}

internal enum Forge
{
    [JsonStringEnumMemberName("github")]
    GitHub,
}

internal enum ReferenceType
{
    [JsonStringEnumMemberName("branch")]
    Branch,

    [JsonStringEnumMemberName("release")]
    Release,
    
    [JsonStringEnumMemberName("tag")]
    Tag,
}
