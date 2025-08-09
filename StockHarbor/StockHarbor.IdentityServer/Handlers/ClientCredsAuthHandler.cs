using Duende.AccessTokenManagement;
using System.Net.Http.Headers;

namespace StockHarbor.IdentityServer.Handlers;

public sealed class ClientCredsAuthHandler : DelegatingHandler
{
    private readonly IClientCredentialsTokenManagementService _tokens;
    public ClientCredsAuthHandler(IClientCredentialsTokenManagementService tokens) => _tokens = tokens;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var token = await _tokens.GetAccessTokenAsync("tenant-api", cancellationToken: ct);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        return await base.SendAsync(request, ct);
    }
}
