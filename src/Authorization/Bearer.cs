using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace Pomni.Authorization;

public class BearerAuthenticationProvider : IAuthenticationProvider
{
    public string? ApiKey { get; init; }

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
