using System.Text;
using Pomni.Client.Codeberg.Models;
using Pomni.Client.Codeberg.Repos.Item.Item.Pulls;
using Pomni.Commands.Update;

namespace Pomni.Commands.Bot;

internal static partial class Bot
{
    public static class Codeberg
    {
        private static readonly string[] Repository = (
            Environment.GetEnvironmentVariable("GITHUB_REPOSITORY")
            ?? throw new InvalidOperationException("GITHUB_REPOSITORY is null")
        ).Split('/');

        private static readonly string Base =
            Environment.GetEnvironmentVariable("GITHUB_REF_NAME")
            ?? throw new InvalidOperationException("GITHUB_REF_NAME is null");

        private static readonly string Head = $"{Repository[0]}:pomni/bot";

        public static async Task RaiseOrModifyPullRequest(UpdatedPin[] updatedPins)
        {
            var pullRequest = await GetPullRequest();

            if (pullRequest != null)
                await ModifyPullRequest((int)pullRequest.Number, updatedPins);
            else
                await RaisePullRequest(updatedPins);
        }

        private static async Task<PullRequest?> GetPullRequest()
        {
            var pullRequests = await Update
                .Codeberg.Client.Repos[Repository[0]][Repository[1]]
                .Pulls.GetAsync(x =>
                {
                    x.QueryParameters.State = GetStateQueryParameterType.Open;
                    x.QueryParameters.Head = Head;
                    x.QueryParameters.Base = Base;
                });

            var pullRequest = pullRequests.ToArray();

            return pullRequest.Length == 1 ? pullRequest[0] : null;
        }

        private static async Task RaisePullRequest(UpdatedPin[] updatedPins)
        {
            var pullRequest = await Update
                .Codeberg.Client.Repos[Repository[0]][Repository[1]]
                .Pulls.PostAsync(
                    new CreatePullRequestOption
                    {
                        Title = Title,
                        Head = Head,
                        Base = Base,
                        Body = WritePullRequestBody(updatedPins),
                    }
                );

            await Console.Out.WriteLineAsync($"Pull request: {pullRequest.Url}");
        }

        private static async Task ModifyPullRequest(int pullNumber, UpdatedPin[] updatedPins)
        {
            var pullRequest = await Update
                .Codeberg.Client.Repos[Repository[0]][Repository[1]]
                .Pulls[pullNumber]
                .PatchAsync(
                    new EditPullRequestOption
                    {
                        Title = Title,
                        Body = WritePullRequestBody(updatedPins),
                        Base = Base,
                    }
                );

            await Console.Out.WriteLineAsync($"Pull request: {pullRequest.Url}");
        }

        private static string WritePullRequestBody(UpdatedPin[] updatedPins)
        {
            var stringBuilder = new StringBuilder();

            foreach (var pin in updatedPins)
                if (pin.OldRev is not null)
                    stringBuilder.AppendLine(
                        $"- {pin.Pin}: [{pin.OldRev} -> {pin.NewRev}](https://codeberg.org/{pin.Repository}/compare/{pin.OldRev}...{pin.NewRev})"
                    );
                else
                    stringBuilder.AppendLine(
                        $"- {pin.Pin}: init at [{pin.NewRev}](https://codeberg.org/{pin.Repository}/tree/{pin.NewRev})"
                    );

            return stringBuilder.ToString();
        }
    }
}
