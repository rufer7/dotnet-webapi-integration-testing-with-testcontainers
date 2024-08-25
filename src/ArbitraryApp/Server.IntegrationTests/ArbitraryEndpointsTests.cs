using ArbitraryApp.Server.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using ArbitraryApp.Server.IntegrationTests.Extensions;
using ArbitraryApp.Server.Models;
using ArbitraryApp.Shared.Authorization;

namespace ArbitraryApp.Server.IntegrationTests;

[Trait("Category", "IntegrationTests")]
[Collection("CustomWebApplicationFactoryCollection")]
public class ArbitraryEndpointsTests
{
    private readonly CustomWebApplicationFactory _factory;

    public ArbitraryEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_AsUnauthenticatedUser_ReturnsUnauthorized()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/Arbitrary");

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Get_AsAuthenticatedUser_ReturnsRecords()
    {
        await _factory.InitializeDatabaseAsync();

        var client = _factory.CreateLoggedInClient<ImpersonatedAuthHandler>(
            new WebApplicationFactoryClientOptions { AllowAutoRedirect = false },
            null);

        var response = await client.GetAsync("/api/Arbitrary");

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var records = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
        Assert.NotNull(records);
        Assert.Equal(1, records.Count());
    }

    [Fact]
    public async Task Post_AsAuthenticatedUser_ReturnsForbidden()
    {
        var client = _factory.CreateLoggedInClient<ImpersonatedAuthHandler>(
            new WebApplicationFactoryClientOptions { AllowAutoRedirect = false },
            null);

        var response = await client.PostAsJsonAsync(
            "/api/Arbitrary",
            new ArbitraryRecordRequestModel());

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Post_AsAdmin_CreatesRecord_AndReturnsSuccess()
    {
        var client = _factory.CreateLoggedInClient<ImpersonatedAuthHandler>(
            new WebApplicationFactoryClientOptions { AllowAutoRedirect = false },
            Roles.Admin);

        var response = await client.PostAsJsonAsync(
            "/api/Arbitrary",
            new ArbitraryRecordRequestModel
            {
                Name = "TestName",
                Value = "TestValue"
            });

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
