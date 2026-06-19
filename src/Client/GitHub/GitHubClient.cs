using System.Text.Json.Serialization;

namespace Pomni.Client.GitHub;

internal partial class GitHubClient : IDisposable
{
    private readonly HttpClient _httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://github.com/"),
        DefaultRequestHeaders =
        {
            { "Accept", "application/vnd.github+json" },
            { "X-GitHub-Api-Version", "2026-03-10" },
        },
    };

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}

[JsonSerializable(typeof(Repository))]
[JsonSerializable(typeof(Branch))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
internal partial class GitHubClientSourceGenerationContext : JsonSerializerContext;
