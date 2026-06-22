using System.Text.Json;
using Pomni.Model;

namespace Pomni.Commands;

internal static class Remove
{
    public static void RemoveRepository(string name)
    {
        var pomniJson = JsonSerializer.Deserialize<PomniPins>(
            File.ReadAllText("pomni/pomni.json"),
            SourceGenerationContext.Default.PomniPins
        );

        pomniJson.Pins.Remove(name);

        using var pomniJsonFile = File.Open("pomni/pomni.json", FileMode.Truncate);

        pomniJsonFile.Write(
            JsonSerializer.SerializeToUtf8Bytes(
                pomniJson,
                SourceGenerationContext.Default.PomniPins
            )
        );
    }
}
