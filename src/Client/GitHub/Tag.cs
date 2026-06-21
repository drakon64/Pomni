using System.Net.Http.Json;

namespace Pomni.Client.GitHub;

internal class Tag
{
    public required Commit Object { get; init; }
}

internal partial class GitHubClient
{
    private static async Task<string> GetTag(string repo, string tag)
    {
        using var request = await HttpClient.SendAsync(
            new HttpRequestMessage
            {
                RequestUri = new Uri($"repos/{repo}/git/ref/tags/{tag}", UriKind.Relative),
            }
        );

        var response = await request.Content.ReadFromJsonAsync<Tag>(
            GitHubClientSourceGenerationContext.Default.Tag
        );

        return response.Object.Sha;
    }
}
