using SW.Core.Entities;
using SW.Core.Services;
using SW.Core.Contracts;
using SW.Infrastructure.Services;
using Moq;

namespace SW.Infrastructure.Tests.Unit.Services;

public class RebateServiceTests
{
    private Mock<IRebateCalculator> _rebateCalculatorMock;

    RebateService _target;

    public RebateServiceTests()
    {
        _rebateCalculatorMock = new Mock<IRebateCalculator>();

        _target = new RebateService(_rebateCalculatorMock.Object);
    }

    [Fact]
    public void OnHappyPath_FixedRateRebate_ShouldSuccess()
    {
        // Arrange
        var response = new CalculateRebateResponse { Success = true };

        _rebateCalculatorMock.Setup(m => m.Calculate(
            It.IsAny<CalculateRebateRequest>(),
            It.IsAny<Product>(),
            It.IsAny<Rebate>()))
            .Returns(response);

        // Act
        var result = _target.Calculate(new CalculateRebateRequest());

        // Assert
        Assert.True(result.Success);
    }

    [Fact(Skip="Data stores not mocked yet")]
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

    [Fact]
    public void OnWrongCase_FixedRateRebate_ShouldFail()
    {
        // Arrange
        var response = new CalculateRebateResponse { Success = false };

        _rebateCalculatorMock.Setup(m => m.Calculate(
            It.IsAny<CalculateRebateRequest>(),
            It.IsAny<Product>(),
            It.IsAny<Rebate>()))
            .Returns(response);

        // Act
        var result = _target.Calculate(new CalculateRebateRequest());

        // Assert
        Assert.False(result.Success);
    }
}
