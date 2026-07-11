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
        using var defaultNix = File.Open("pomni/default.nix", FileMode.CreateNew);

        pomniJson.Write(
            JsonSerializer.SerializeToUtf8Bytes<PomniPins>(
                new PomniPins { Version = 1, Pins = new Dictionary<string, PomniPin>() },
                SourceGenerationContext.Default.PomniPins
            )
        );

        pomniLockJson.Write("{}"u8);

        // TODO: Read from a file at compile time
        defaultNix.Write(
            """
builtins.mapAttrs (
  name: args:
  (builtins.fetchTarball {
    url = args.url;
    sha256 = args.hash;
  })
) (builtins.fromJSON (builtins.readFile ./pomni.lock.json))

"""u8
        );
    }
}
