namespace pomni.Model;

internal class PomniJson
{
    public required string Url { get; init; }
    public required string Revision { get; init; }
    public required ReferenceType ReferenceType { get; init; }

    public string? Reference { get; init; } = null;
    public bool Frozen { get; init; } = false;
}

internal enum ReferenceType
{
    Commit,
    Release,
    Tag,
}
