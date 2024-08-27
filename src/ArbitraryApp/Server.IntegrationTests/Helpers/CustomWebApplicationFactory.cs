using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using System.Net.Http.Headers;
using System.Security.Claims;
using ArbitraryApp.Domain;
using ArbitraryApp.Server.IntegrationTests.Options;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace ArbitraryApp.Server.IntegrationTests.Helpers;

/// <summary>
/// WebApplicationFactory<Program> is used to create an
/// in-memory TestServer for the integration tests
/// 
/// Entry point (generic type argument) is Program.cs 
/// so that the test server will be set up similar as in production scenario
/// 
/// see http://www.tiernok.com/posts/2021/mocking-oidc-logins-for-integration-tests/
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly MsSqlContainer _msSqlContainer;

    public CustomWebApplicationFactory()
    {
        _msSqlContainer = new MsSqlBuilder()
            .WithPortBinding(1433, true)
            .Build();

        _msSqlContainer
            .StartAsync()
            .GetAwaiter()
            .GetResult();
    }

    public HttpClient CreateLoggedInClient<T>(
        WebApplicationFactoryClientOptions options,
        string? role)
        where T : ImpersonatedAuthHandler
    {
        // Enforce HTTPS in integration tests
        options.BaseAddress = new Uri("https://localhost");
        return CreateLoggedInClient<T>(options, list =>
        {
            if (!string.IsNullOrWhiteSpace(role))
            {
                list.Add(new Claim(ClaimTypes.Role, role));
            }
        });
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<ApplicationDbContext>));
            if (dbContextDescriptor is not null)
            {
                services.Remove(dbContextDescriptor);
            }

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbConnection));
            if (dbConnectionDescriptor is not null)
            {
                services.Remove(dbConnectionDescriptor);
            }

            services.AddSingleton<DbConnection>(_ =>
            {
                var connection =
                    new SqlConnection(
                        $"server={_msSqlContainer.Hostname},{_msSqlContainer.GetMappedPublicPort(1433)};" +
                        $"user id={MsSqlBuilder.DefaultUsername};password={MsSqlBuilder.DefaultPassword};" +
                        "database=Test;" +
                        "Encrypt=false;Persist Security Info=true");

                return connection;
            });

            services.AddDbContext<ApplicationDbContext>((container, optionsBuilder) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                optionsBuilder.UseSqlServer(connection);
            });

            var antiForgery = services.SingleOrDefault(
                d => d.ServiceType == typeof(IAntiforgery));

            if (antiForgery is not null)
            {
                services.Remove(antiForgery);
            }

            services.AddTransient<IAntiforgery, TrueIfTestSchemeAntiForgery>();
        });

        base.ConfigureWebHost(builder);
    }

    private HttpClient CreateLoggedInClient<T>(
        WebApplicationFactoryClientOptions options,
        Action<List<Claim>> configure)
        where T : ImpersonatedAuthHandler
    {
        var client = WithWebHostBuilder(builder =>
                builder.ConfigureTestServices(services =>
                {
                    // configure the intercepting provider
                    services.AddTransient<IAuthenticationSchemeProvider, TestAuthenticationSchemeProvider>();

                    services.AddAuthentication(TestAuthenticationSchemeProvider.SchemeName)
                        .AddScheme<ImpersonatedAuthenticationSchemeOptions, T>(
                            TestAuthenticationSchemeProvider.SchemeName, configureOptions =>
                            {
                                configureOptions.Configure = configure;
                            });
                }))
            .CreateClient(options);

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(scheme: TestAuthenticationSchemeProvider.SchemeName);

        return client;
    }

    public override async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);

        await base.DisposeAsync();

        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        await _msSqlContainer.DisposeAsync();
    }
}
