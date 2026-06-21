using Pomni.Client.GitHub;
using Pomni.Model;

namespace Pomni.Commands.Update;

internal partial class Update
{
    private static async Task<PomniLock> UpdateGitHubRepository(PomniPin pomniPin)
    {
        var repo = pomniPin.Repository;
        string sha;

        switch (pomniPin.ReferenceType)
        {
            case ReferenceType.Branch or null:
            {
                string branch;

                if (pomniPin.Reference is not null)
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
                break;
            }
            case ReferenceType.Release:
                sha = await GitHubClient.GetLatestRelease(repo);
                break;
            case ReferenceType.Tag:
                throw new NotImplementedException();
            default:
                throw new ArgumentException();
        }

        var url = $"https://github.com/{pomniPin.Repository}/archive/{sha}.tar.gz";

        return new PomniLock { Url = url, Hash = await GetSri(url) };
    }
}
