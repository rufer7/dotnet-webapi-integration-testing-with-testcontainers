[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace ArbitraryApp.Server.IntegrationTests.Helpers;

[CollectionDefinition("CustomWebApplicationFactoryCollection")]
public class CustomWebApplicationFactoryCollection : ICollectionFixture<CustomWebApplicationFactory>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
