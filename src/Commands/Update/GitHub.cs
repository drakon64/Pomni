using Pomni.Model;

namespace Pomni.Commands.Update;

internal partial class Update
{
    private static async Task<PomniLock> UpdateGitHubRepository(PomniPin pomniPin)
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
                    var getRepo = await Program
                        .GitHubClient.Value.Repos[repo[0]][repo[1]]
                        .GetAsync();

                    branch = getRepo.DefaultBranch;
                }

                var getBranch = await Program
                    .GitHubClient.Value.Repos[repo[0]][repo[1]]
                    .Branches[branch]
                    .GetAsync();

                sha = getBranch.Commit.Sha;
                break;
            }
            case ReferenceType.Release:
                var tag = (
                    await Program
                        .GitHubClient.Value.Repos[repo[0]][repo[1]]
                        .Releases.Latest.GetAsync()
                ).TagName;

                sha = (
                    await Program
                        .GitHubClient.Value.Repos[repo[0]][repo[1]]
                        .Git.Ref[$"tags/{tag}"]
                        .GetAsync()
                )
                    .Object
                    .Sha;

                break;
            default:
                throw new ArgumentException();
        }

        var url = $"https://github.com/{pomniPin.Repository}/archive/{sha}.tar.gz";

        return new PomniLock { Url = url, Hash = await GetSri(url) };
    }
}
