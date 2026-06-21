using System.Text.Json;
using Pomni.Model;

namespace Pomni.Commands;

internal static class Init
{
    public static void InitPomniJson()
    {
        Directory.CreateDirectory("pomni");
        using var pomniJson = File.Open("pomni/pomni.json", FileMode.CreateNew);
        using var pomniLockJson = File.Open("pomni/pomni.lock.json", FileMode.CreateNew);

        pomniJson.Write(
            JsonSerializer.SerializeToUtf8Bytes<PomniPins>(
                new PomniPins { Version = 1, Pins = new Dictionary<string, PomniPin>() },
                SourceGenerationContext.Default.PomniPins
            )
        );

        pomniLockJson.Write("{}"u8);
    }
}
