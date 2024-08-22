namespace ArbitraryApp.Client.Services;

public interface IAntiforgeryHttpClientFactory
{
    Task<HttpClient> CreateClientAsync(string clientName = "authorizedClient");
}
