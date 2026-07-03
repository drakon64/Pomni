using System.Diagnostics;
using System.Text.Json;
using Pomni.Model;

namespace Pomni.Commands.Bot;

internal static class Bot
{
    public static async Task GetStorePaths()
    {
        var pomniJson = JsonSerializer.Deserialize<PomniPins>(
            File.ReadAllText("pomni/pomni.json"),
            SourceGenerationContext.Default.PomniPins
        );

        var changed = false;

        if (pomniJson.Derivations is not null)
        {
            foreach (var derivation in pomniJson.Derivations)
            {
                var instantiateProcessStartInfo = new ProcessStartInfo
                {
                    FileName = "nix-instantiate",
                    ArgumentList = { derivation.Value.Path },
                    RedirectStandardOutput = true,
                };

                var instantiateProcess = Process.Start(instantiateProcessStartInfo);

                await instantiateProcess.WaitForExitAsync();

                derivation.Value.StorePaths = new StorePaths
                {
                    OldStorePaths = [],
                    NewStorePaths = (await instantiateProcess.StandardOutput.ReadToEndAsync())
                        .TrimEnd()
                        .Split('\n'),
                };

                if (
                    derivation.Value.StorePaths.OldStorePaths
                    != derivation.Value.StorePaths.NewStorePaths
                )
                    changed = true;
            }

            if (changed)
            {
                await Console.Out.WriteLineAsync("Store paths changed");
            }
            else
            {
                await Console.Out.WriteLineAsync("Store paths not changed");
            }
        }
        else throw new ArgumentNullException();
    }
}
