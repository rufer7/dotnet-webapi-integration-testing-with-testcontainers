using ArbitraryApp.Server.Cae;
using Microsoft.AspNetCore.Http;

namespace ArbitraryApp.Server.Tests.Cae;

public class CaeClaimsChallengeServiceTests
{
    [Fact]
    public void CheckForRequiredAuthContextIdToken_WithNullAuthContextId_ReturnsNull()
    {
        // Arrange
        var service = new CaeClaimsChallengeService();

        // Act
        var result = service.CheckForRequiredAuthContextIdToken(null, new DefaultHttpContext());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void CheckForRequiredAuthContextIdToken_WithEmptyAuthContextId_ReturnsNull()
    {
        // Arrange
        var service = new CaeClaimsChallengeService();

        // Act
        var result = service.CheckForRequiredAuthContextIdToken(string.Empty, new DefaultHttpContext());

        // Assert
        Assert.Null(result);
    }
}