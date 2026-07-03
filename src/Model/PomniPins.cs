using System.Text.Json.Serialization;

namespace Pomni.Model;

internal class PomniPins
{
    public required byte Version { get; init; }
    public required Dictionary<string, PomniPin> Pins { get; init; }
    public Dictionary<string, Derivation>? Derivations { get; init; }
}

internal class PomniPin
{
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public required Forge Forge { get; set; }

    public required string Repository { get; set; }

    public string? Branch { get; set; }
    public ReferenceType? Type { get; set; }
    public bool Frozen { get; set; }
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
}

internal class Derivation
{
    public required string Path { get; init; }
    public string[]? Attributes { get; init; }
    public bool DiffClosures { get; init; }

    [JsonIgnore]
    public StorePaths? StorePaths { get; set; }
}

internal class StorePaths
{
    public required string[] OldStorePaths { get; init; }
    public required string[] NewStorePaths { get; init; }
}
