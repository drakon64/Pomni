using System.Text.Json;
using Pomni.Client.GitHub;
using Pomni.Model;

namespace Pomni.Commands;

internal static class Update
{
    public static async Task UpdateRepositories()
    {
        await using var pomniJson = File.OpenRead("pomni.json");
        var pomniLockJson = File.OpenRead("pomni.lock.json");

        var pomniPins = await JsonSerializer.DeserializeAsync<Dictionary<string, PomniPin>>(
            pomniJson,
            SourceGenerationContext.Default.DictionaryStringPomniPin
        );

        var pomniLocks = await JsonSerializer.DeserializeAsync<Dictionary<string, PomniLock>>(
            pomniLockJson,
            SourceGenerationContext.Default.DictionaryStringPomniLock
        );

        await pomniLockJson.DisposeAsync();

        var updatedLocks = new Dictionary<string, PomniLock>();

        foreach (var pin in pomniPins)
        {
            if (pin.Value.Frozen is false or null)
            {
                var newLock = pomniLocks.GetValueOrDefault(pin.Key);

                var updatedPin = await UpdateRepository(pin.Value);

                if (newLock is not null)
                    updatedLocks[pin.Key] = updatedPin;
                else
                    updatedLocks.Add(pin.Key, updatedPin);
            }
            else
                updatedLocks[pin.Key] = pomniLocks[pin.Key];
        }

        pomniLockJson = File.Open("pomni.lock.json", FileMode.Truncate);
        await pomniLockJson.WriteAsync(
            JsonSerializer.SerializeToUtf8Bytes(
                updatedLocks,
                SourceGenerationContext.Default.DictionaryStringPomniLock
            )
        );
        await pomniLockJson.DisposeAsync();
    }

    private static async Task<PomniLock> UpdateRepository(PomniPin pomniPin)
    {
        string sha;

        if (pomniPin.ReferenceType is ReferenceType.Branch or null)
        {
            var repo = pomniPin.Repository;

            string branch;

            if (pomniPin is { ReferenceType: ReferenceType.Branch, Reference: not null })
            {
                branch = pomniPin.Reference;
            }
            else
            {
                var getRepo = await GitHubClient.GetRepository(repo);
                branch = getRepo.DefaultBranch;
            }

            var getBranch = await GitHubClient.GetBranch(repo, branch);
            sha = getBranch.Commit.Sha;
        }
        else
            sha = "";

        return new PomniLock
        {
            Url = $"https://github.com/{pomniPin.Repository}/archive/{sha}.tar.gz",
            Hash = "",
        };
    }
}
