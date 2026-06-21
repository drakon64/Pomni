using System.Text.Json.Serialization;

namespace Pomni.Model;

[JsonSerializable(typeof(PomniPins))]
[JsonSerializable(typeof(Dictionary<string, PomniLock>))]
[JsonSourceGenerationOptions(
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    UseStringEnumConverter = true,
    WriteIndented = true
)]
internal partial class SourceGenerationContext : JsonSerializerContext;
