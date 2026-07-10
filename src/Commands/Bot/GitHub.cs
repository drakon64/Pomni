using Pomni.Client.GitHub.Models;
using Pomni.Client.GitHub.Repos.Item.Item.Pulls;
using Pomni.Client.GitHub.Repos.Item.Item.Pulls.Item;

namespace Pomni.Commands.Bot;

internal static partial class Bot
{
    public static class GitHub
    {
        private static readonly string[] Repository = Environment
            .GetEnvironmentVariable("GITHUB_REPOSITORY")
            .Split('/');

        private static readonly string Base =
            Environment.GetEnvironmentVariable("GITHUB_REF_NAME")
            ?? throw new InvalidOperationException("GITHUB_REF_NAME is null");

        private static readonly string Head = $"{Repository[0]}:pomni/bot";

        public static async Task RaiseOrModifyPullRequest()
        {
            var pullRequest = await GetPullRequest();

            if (pullRequest != null)
                await ModifyPullRequest((int)pullRequest.Number);
            else
                await RaisePullRequest();
        }

        private static async Task<PullRequestSimple?> GetPullRequest()
        {
            var pullRequests = await Program
                .GitHubClient.Value.Repos[Repository[0]][Repository[1]]
                .Pulls.GetAsync(x =>
                {
                    x.QueryParameters.State = GetStateQueryParameterType.Open;
                    x.QueryParameters.Head = Head;
                    x.QueryParameters.Base = Base;
                    x.QueryParameters.PerPage = 1;
                });

            var pullRequest = pullRequests.ToArray();

            return pullRequest.Length == 1 ? pullRequest[0] : null;
        }

        private static async Task RaisePullRequest()
        {
            var pullRequest = await Program
                .GitHubClient.Value.Repos[Repository[0]][Repository[1]]
                .Pulls.PostAsync(
                    new PullsPostRequestBody
                    {
                        Title = Title,
                        Head = Head,
                        Base = Base,
                    }
                );

            await Console.Out.WriteLineAsync($"Pull request: {pullRequest.Url}");
        }

        private static async Task ModifyPullRequest(int pullNumber)
        {
            var pullRequest = await Program
                .GitHubClient.Value.Repos[Repository[0]][Repository[1]]
                .Pulls[pullNumber]
                .PatchAsync(new WithPull_numberPatchRequestBody { Title = Title, Base = Base });

            await Console.Out.WriteLineAsync($"Pull request: {pullRequest.Url}");
        }
    }
}
