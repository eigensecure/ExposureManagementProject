using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace CloudAccountsUI.Services;

public class JwtTokenHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;

    public JwtTokenHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        var username = await _localStorage.GetItemAsync<string>("username");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        if (!string.IsNullOrEmpty(username))
        {
            request.Headers.Add("X-Username", username);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}