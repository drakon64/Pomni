using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Pomni.Client.Codeberg;
using Pomni.Model;

namespace Pomni.Commands.Update;

internal static class Codeberg
{
    private static readonly BearerAuthenticationProvider AuthProvider = new()
    {
        ApiKey = Environment.GetEnvironmentVariable("BEARER_TOKEN"),
    };
    private static readonly HttpClientRequestAdapter Adapter = new(AuthProvider);
    internal static readonly CodebergClient Client = new(Adapter);

    internal static async Task<PomniLock> UpdateRepository(PomniPin pomniPin)
    {
        var repo = pomniPin.Repository.Split('/');
        string sha;

        switch (pomniPin.Type)
        {
            case ReferenceType.Branch or null:
            {
                string branch;

                if (pomniPin.Branch is not null)
                {
                    branch = pomniPin.Branch;
                }
                else
                {
                    var getRepo = await Client.Repos[repo[0]][repo[1]].GetAsync();

                    branch = getRepo.DefaultBranch;
                }

                var getBranch = await Client.Repos[repo[0]][repo[1]].Branches[branch].GetAsync();

                sha = getBranch.Commit.Id;
                break;
            }
            case ReferenceType.Release:
                var tag = (await Client.Repos[repo[0]][repo[1]].Releases.Latest.GetAsync()).TagName;

                sha = (await Client.Repos[repo[0]][repo[1]].Git.Refs[$"tags/{tag}"].GetAsync())[0]
                    .Object
                    .Sha;

                break;
            default:
                throw new ArgumentException();
        }

        var url = $"https://codeberg.org/{pomniPin.Repository}/archive/{sha}.tar.gz";

        return new PomniLock { Url = url, Hash = await Update.GetSri(url) };
    }

    private class BearerAuthenticationProvider : IAuthenticationProvider
    {
        internal string? ApiKey { get; init; }

        public Task AuthenticateRequestAsync(
            RequestInformation request,
            Dictionary<string, object>? additionalAuthenticationContext = null,
            CancellationToken cancellationToken = new()
        )
        {
            if (ApiKey is not null)
                request.Headers.Add("Authorization", $"Bearer {ApiKey}");

            return Task.CompletedTask;
        }
    }
}
