using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace ArbitraryApp.Server.IntegrationTests.Options;

public class ImpersonatedAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public Action<List<Claim>> Configure { get; set; } = _ => { };
}
