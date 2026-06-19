using System.Net.Http.Json;

namespace Pomni.Client.GitHub;

internal class Branch
{
    public required Commit Commit { get; init; }
}

internal class Commit
{
    public required string Sha { get; init; }
}

internal partial class GitHubClient
{
    public async Task<Branch> GetBranch(string repo, string branch)
    {
        var request = await _httpClient.SendAsync(
            new HttpRequestMessage { RequestUri = new Uri($"repos/{repo}/branches/{branch}") }
        );

        var response = await request.Content.ReadFromJsonAsync<Branch>(
            GitHubClientSourceGenerationContext.Default.Branch
        );

        return response;
    }
}
