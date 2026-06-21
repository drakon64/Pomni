using System.Net.Http.Json;

namespace Pomni.Client.GitHub;

internal class Release
{
    public required string TagName { get; init; }
}

internal partial class GitHubClient
{
    public static async Task<string> GetLatestRelease(string repo)
    {
        using var request = await HttpClient.SendAsync(
            new HttpRequestMessage
            {
                RequestUri = new Uri($"repos/{repo}/releases/latest", UriKind.Relative),
            }
        );

        var response = await request.Content.ReadFromJsonAsync<Release>(
            GitHubClientSourceGenerationContext.Default.Release
        );

        var tag = await GetTag(repo, response.TagName);

        return tag;
    }
}
