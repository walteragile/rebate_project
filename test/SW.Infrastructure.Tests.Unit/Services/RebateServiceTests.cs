using SW.Core.Services;
using SW.Infrastructure.Services;

namespace SW.Infrastructure.Tests.Unit.Services;

public class RebateServiceTests
{
    RebateService _target;

    public RebateServiceTests()
    {
        _target = new RebateService();
    }

    [Fact]
    public void OnHappyPath_FixedRateRebate_ShouldSuccess()
    {
        // Arrange
        var request = new CalculateRebateRequest
        {
            ProductIdentifier = "P1",
            RebateIdentifier = "R1",
            Volume = 10m,
        };

        // Act
        var result = _target.Calculate(request);

        // Assert
        Assert.True(result.Success);
    }

    [Fact]
    public void OnZeroPercentage_FixedRateRebate_ShouldFail()
    {
        // Arrange
        var request = new CalculateRebateRequest
        {
            ProductIdentifier = "P1",
            RebateIdentifier = "R2",
            Volume = 10m,
        };

        // Act
        var result = _target.Calculate(request);

        // Assert
        Assert.False(result.Success);
    }
}
