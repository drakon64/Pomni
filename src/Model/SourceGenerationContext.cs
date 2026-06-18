using System.Text.Json.Serialization;

namespace pomni.Model;

[JsonSerializable(typeof(Dictionary<string, PomniJson>))]
[JsonSerializable(typeof(Dictionary<string, PomniLockJson>))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    UseStringEnumConverter = true
)]
internal partial class SourceGenerationContext : JsonSerializerContext;
