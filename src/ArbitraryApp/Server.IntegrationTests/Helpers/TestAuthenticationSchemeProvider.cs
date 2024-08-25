using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ArbitraryApp.Server.IntegrationTests.Helpers;

/// <summary>
/// This class is used to replace the built-in AuthenticationSchemeProvider with one
/// that intercepts requests for all schemes and returns the scheme with name "TestScheme"
/// </summary>
public class TestAuthenticationSchemeProvider : AuthenticationSchemeProvider
{
    public const string SchemeName = "TestScheme";

    public TestAuthenticationSchemeProvider(IOptions<AuthenticationOptions> options)
        : base(options)
    {
    }

    protected TestAuthenticationSchemeProvider(IOptions<AuthenticationOptions> options, IDictionary<string, AuthenticationScheme> schemes)
        : base(options, schemes)
    {
    }

    public override Task<AuthenticationScheme?> GetSchemeAsync(string name)
    {
        return base.GetSchemeAsync(SchemeName);
    }
}