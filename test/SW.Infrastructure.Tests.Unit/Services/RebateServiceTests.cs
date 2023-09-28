using SW.Core.Types;
using SW.Core.Entities;
using SW.Core.Services;
using SW.Core.Contracts;
using SW.Infrastructure.Services;
using Moq;
using SW.Infrastructure.RebateCalculators;

namespace SW.Infrastructure.Tests.Unit.Services;

public class RebateServiceTests
{
    private readonly Mock<IRebateCalculator> _rebateCalculatorMock;
    private readonly Mock<IRepository<Product>> _productRepositoryMock;
    private readonly Mock<IRepository<Rebate>> _rebateRepositoryMock;

    RebateService _target;

    public RebateServiceTests()
    {
        _rebateCalculatorMock = new Mock<IRebateCalculator>();
        _productRepositoryMock = new Mock<IRepository<Product>>();
        _rebateRepositoryMock = new Mock<IRepository<Rebate>>();

        _target = new RebateService(_rebateCalculatorMock.Object, _productRepositoryMock.Object, _rebateRepositoryMock.Object);
    }

    [Fact]
    public void OnHappyPath_FixedRateRebate_ShouldSuccess()
    {
        // Arrange
        var response = new CalculateRebateResponse { Success = true };
        var product = new Product
        {
            Identifier = "P1",
            Price = 10m,
            Uom = "uom",
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
        };
        var rebate = new Rebate
        {
            Identifier = "R1",
            Incentive = IncentiveType.FixedRateRebate,
            Amount = 2m,
            Percentage = 0.1m,
        };

        _productRepositoryMock.Setup(m => m.Get(It.IsAny<string>()))
            .Returns(product);
        _rebateRepositoryMock.Setup(m => m.Get(It.IsAny<string>()))
            .Returns(rebate);

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
        var product = new Product
        {
            Identifier = "P1",
            Price = 10m,
            Uom = "uom",
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
        };
        var rebate = new Rebate
        {
            Identifier = "R1",
            Incentive = IncentiveType.FixedRateRebate,
            Amount = 2m,
            Percentage = 0.0m,
        };

        _productRepositoryMock.Setup(m => m.Get(It.IsAny<string>()))
            .Returns(product);
        _rebateRepositoryMock.Setup(m => m.Get(It.IsAny<string>()))
            .Returns(rebate);

        _target = new RebateService(new FixedRateRebateCalculator(), _productRepositoryMock.Object, _rebateRepositoryMock.Object);

        // Act
        var result = _target.Calculate(request);

        // Assert
        Assert.False(result.Success);
        _rebateRepositoryMock.Verify(m => m.Save(It.IsAny<Rebate>()), Times.Never());
    }

    [Fact]
    public void OnNotFoundProduct_FixedRateRebate_ShouldFail()
    {
        // Arrange
        const string WRONG_PRODUCT_IDENTIFIER = "P1";
        var request = new CalculateRebateRequest
        {
            ProductIdentifier = WRONG_PRODUCT_IDENTIFIER,
            RebateIdentifier = "R2",
            Volume = 10m,
        };
        _productRepositoryMock.Setup(m => m.Get(WRONG_PRODUCT_IDENTIFIER))
            .Returns((Product)null);

        // Act
        var result = _target.Calculate(request);

        // Assert
        Assert.False(result.Success);
    }
}
