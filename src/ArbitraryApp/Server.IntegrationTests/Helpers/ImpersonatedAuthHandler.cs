using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using ArbitraryApp.Server.IntegrationTests.Options;

namespace ArbitraryApp.Server.IntegrationTests.Helpers;

public class ImpersonatedAuthHandler : AuthenticationHandler<ImpersonatedAuthenticationSchemeOptions>
{
    public ImpersonatedAuthHandler(
        IOptionsMonitor<ImpersonatedAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new List<Claim>()
        {
            new ("name", "Arbitrary user")
        };
        Options.Configure(claims);
        var identity = new ClaimsIdentity(claims, TestAuthenticationSchemeProvider.SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, TestAuthenticationSchemeProvider.SchemeName);

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}