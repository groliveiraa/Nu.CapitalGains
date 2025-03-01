using CapitalGains.Application.Services;
using CapitalGains.Domain.Enums;
using CapitalGains.Domain.Models;

namespace CapitalGains.Tests.UnitTests;

public class TaxCalculatorServiceTests
{
    [Fact]
    public void Calculate_BuyOperation_ReturnsZeroTaxAndUpdatesWeightedAverage()
    {
        // Arrange
        var calculator = new TaxCalculatorService();
        var operation = new Operations("buy", 10.00m, 10000) { OperationType = OperationType.Buy };
        decimal weightedAverage = 0m;
        int totalShares = 0;
        decimal accumulatedLoss = 0m;

        // Act
        var result = calculator.Calculate(operation, ref weightedAverage, ref totalShares, ref accumulatedLoss);

        // Assert
        Assert.Equal(0m, result.Tax);
        Assert.Equal(10.00m, weightedAverage);
        Assert.Equal(10000, totalShares);
        Assert.Equal(0m, accumulatedLoss);
    }

    [Fact]
    public void Calculate_SellWithProfitAboveThreshold_ReturnsCorrectTax()
    {
        // Arrange
        var calculator = new TaxCalculatorService();
        var operation = new Operations("sell", 20.00m, 5000) { OperationType = OperationType.Sell };
        decimal weightedAverage = 10.00m;
        int totalShares = 10000;
        decimal accumulatedLoss = 0m;

        // Act
        var result = calculator.Calculate(operation, ref weightedAverage, ref totalShares, ref accumulatedLoss);

        // Assert
        Assert.Equal(10000.00m, result.Tax); // (20.00 - 10.00) * 5000 * 0.2 = 10000.00
        Assert.Equal(10.00m, weightedAverage);
        Assert.Equal(5000, totalShares);
        Assert.Equal(0m, accumulatedLoss);
    }

    [Fact]
    public void Calculate_SellWithLoss_AccumulatesLossAndReturnsZeroTax()
    {
        // Arrange
        var calculator = new TaxCalculatorService();
        var operation = new Operations("sell", 5.00m, 5000) { OperationType = OperationType.Sell };
        decimal weightedAverage = 10.00m;
        int totalShares = 10000;
        decimal accumulatedLoss = 0m;

        // Act
        var result = calculator.Calculate(operation, ref weightedAverage, ref totalShares, ref accumulatedLoss);

        // Assert
        Assert.Equal(0m, result.Tax);
        Assert.Equal(10.00m, weightedAverage);
        Assert.Equal(5000, totalShares);
        Assert.Equal(25000.00m, accumulatedLoss); // (5.00 - 10.00) * 5000 = -25000.00
    }

    [Fact]
    public void Calculate_SellBelowThreshold_ReturnsZeroTax()
    {
        // Arrange
        var calculator = new TaxCalculatorService();
        var operation = new Operations("sell", 15.00m, 50) { OperationType = OperationType.Sell };
        decimal weightedAverage = 10.00m;
        int totalShares = 100;
        decimal accumulatedLoss = 0m;

        // Act
        var result = calculator.Calculate(operation, ref weightedAverage, ref totalShares, ref accumulatedLoss);

        // Assert
        Assert.Equal(0m, result.Tax); // 15.00 * 50 = 750.00 < 20000, so no tax
        Assert.Equal(10.00m, weightedAverage);
        Assert.Equal(50, totalShares);
        Assert.Equal(0m, accumulatedLoss);
    }
}
