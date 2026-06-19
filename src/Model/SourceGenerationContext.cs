using System.Text.Json.Serialization;

namespace Pomni.Model;

[JsonSerializable(typeof(Dictionary<string, PomniJson>))]
[JsonSerializable(typeof(Dictionary<string, PomniLockJson>))]
[JsonSourceGenerationOptions(
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    UseStringEnumConverter = true,
    WriteIndented = true
)]
internal partial class SourceGenerationContext : JsonSerializerContext;
