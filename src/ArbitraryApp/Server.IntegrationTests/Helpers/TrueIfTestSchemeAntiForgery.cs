using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace ArbitraryApp.Server.IntegrationTests.Helpers;

internal class TrueIfTestSchemeAntiForgery : IAntiforgery
{
    private const string AntiForgeryHeaderName = "X-XSRF-TOKEN";

    public AntiforgeryTokenSet GetAndStoreTokens(HttpContext httpContext)
    {
        return new AntiforgeryTokenSet(null, null, "__RequestVerificationToken", AntiForgeryHeaderName);
    }

    public AntiforgeryTokenSet GetTokens(HttpContext httpContext)
    {
        return new AntiforgeryTokenSet(null, null, "__RequestVerificationToken", AntiForgeryHeaderName);
    }

    public Task<bool> IsRequestValidAsync(HttpContext httpContext)
    {
        var authProperties = httpContext.Features.GetRequiredFeature<IAuthenticateResultFeature>();
        var authScheme = authProperties.AuthenticateResult!.Ticket!.Properties.Items[".AuthScheme"];
        return Task.FromResult(authScheme is TestAuthenticationSchemeProvider.SchemeName);
    }

    public Task ValidateRequestAsync(HttpContext httpContext)
    {
        return Task.CompletedTask;
    }

    public void SetCookieTokenAndHeader(HttpContext httpContext)
    {
    }
}
