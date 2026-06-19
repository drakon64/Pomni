using System.Net.Http.Json;

namespace Pomni.Client.GitHub;

internal class Repository
{
    public required string DefaultBranch { get; init; }
}

internal partial class GitHubClient
{
    public async Task<Repository> GetRepository(string repo)
    {
        var request = await _httpClient.SendAsync(
            new HttpRequestMessage { RequestUri = new Uri($"repos/{repo}") }
        );

        var response = await request.Content.ReadFromJsonAsync<Repository>(
            GitHubClientSourceGenerationContext.Default.Repository
        );

        return response;
    }
}
