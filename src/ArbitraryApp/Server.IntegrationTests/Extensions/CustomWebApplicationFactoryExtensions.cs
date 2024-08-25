using ArbitraryApp.Domain;
using ArbitraryApp.Server.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ArbitraryApp.Server.IntegrationTests.Extensions;

internal static class CustomWebApplicationFactoryExtensions
{
    public static async Task<InitialData> InitializeDatabaseAsync(this CustomWebApplicationFactory factory)
    {
        await using var scope = factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // await dbContext.Database.EnsureDeletedAsync() not used
        // since EnsureCreatedAsync() then recreates the database every time,
        // making integration tests run take too long
        await dbContext.Database.EnsureCreatedAsync();

        // Delete all entities in reverse dependency order
        await dbContext.ArbitraryRecords.ExecuteDeleteAsync();

        var initialArbitraryRecord = await dbContext.AddArbitraryRecordAsync(
            TestConstants.Name,
            TestConstants.Value);

        return new InitialData(
            initialArbitraryRecord.Id,
            initialArbitraryRecord.Name,
            initialArbitraryRecord.Value);
    }

    public class InitialData
    {
        public InitialData(long id, string name, string value)
        {
            Id = id;
            Name = name;
            Value = value;
        }

        public long Id { get; }
        public string Name { get; }
        public string Value { get; }
    }

    public static async Task<ArbitraryRecord> AddArbitraryRecordAsync(
        this ApplicationDbContext dbContext,
        string name,
        string value)
    {
        var arbitraryRecordEntity = new ArbitraryRecord
        {
            Name = name,
            Value = value
        };

        dbContext.ArbitraryRecords.Add(arbitraryRecordEntity);
        await dbContext.SaveChangesAsync();

        return arbitraryRecordEntity;
    }
}
