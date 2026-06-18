using System.Text.Json;
using pomni.Model;

namespace Pomni.Commands;

internal static class Add
{
    public static void AddRepository(string name, string url)
    {
        var pomniJson = JsonSerializer.Deserialize<Dictionary<string, PomniJson>>(
            File.ReadAllText("pomni.json"),
            SourceGenerationContext.Default.DictionaryStringPomniJson
        );

        pomniJson.Add(
            name,
            new PomniJson
            {
                Url = url,
                Revision = "",
                ReferenceType = ReferenceType.Commit,
            }
        );

        using var pomniJsonFile = File.Open("pomni.json", FileMode.Create);

        pomniJsonFile.Write(
            JsonSerializer.SerializeToUtf8Bytes(
                pomniJson,
                SourceGenerationContext.Default.DictionaryStringPomniJson
            )
        );
    }
}
