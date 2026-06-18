using System.Text.Json;
using pomni.Model;

namespace Pomni.Commands;

internal static class Init
{
    public static void InitPomniJson()
    {
        using var pomniJsonFile = File.OpenWrite("pomni.json");

        pomniJsonFile.Write(
            JsonSerializer.SerializeToUtf8Bytes<Dictionary<string, PomniJson>>(
                new Dictionary<string, PomniJson>(),
                SourceGenerationContext.Default.DictionaryStringPomniJson
            )
        );
    }
}
