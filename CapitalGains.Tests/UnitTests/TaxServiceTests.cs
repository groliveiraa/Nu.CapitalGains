using CapitalGains.Application.Services;
using CapitalGains.Domain.Enums;
using CapitalGains.Domain.Models;
using System.Collections.ObjectModel;

namespace CapitalGains.Tests.UnitTests;

public class TaxServiceTests
{
    [Fact]
    public void CalculateTaxes_Case2_ReturnsExpectedTaxes()
    {
        // Arrange - Caso #2
        var taxService = new TaxService();
        var operations = new Collection<Operations>
        {
            new Operations("buy", 10.00m, 10000) { OperationType = OperationType.Buy },
            new Operations("sell", 20.00m, 5000) { OperationType = OperationType.Sell }
        };

        // Act
        var results = taxService.CalculateTaxes(operations);

        // Assert
        Assert.Equal(2, results.Count);
        Assert.Equal(0m, results[0].Tax);
        Assert.Equal(10000.00m, results[1].Tax);
    }

    [Fact]
    public void CalculateTaxes_Case3_WithLossDeduction_ReturnsExpectedTaxes()
    {
        // Arrange - Caso #3
        var taxService = new TaxService();
        var operations = new Collection<Operations>
        {
            new Operations("buy", 10.00m, 10000) { OperationType = OperationType.Buy },
            new Operations("sell", 5.00m, 5000) { OperationType = OperationType.Sell },
            new Operations("sell", 20.00m, 3000) { OperationType = OperationType.Sell }
        };

        // Act
        var results = taxService.CalculateTaxes(operations);

        // Assert
        Assert.Equal(3, results.Count);
        Assert.Equal(0m, results[0].Tax);
        Assert.Equal(0m, results[1].Tax);
        Assert.Equal(1000.00m, results[2].Tax); // (20.00 - 10.00) * 3000 - 25000 = 5000 * 0.2 = 1000.00
    }

    [Fact]
    public void CalculateTaxes_NullOperations_ThrowsArgumentNullException()
    {
        // Arrange
        var taxService = new TaxService();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => taxService.CalculateTaxes(null));
    }

    [Fact]
    public void CalculateTaxes_Case7_WithMultipleOperations_ReturnsExpectedTaxes()
    {
        // Arrange - Caso #7
        var taxService = new TaxService();
        var operations = new Collection<Operations>
        {
            new Operations("buy", 10.00m, 10000) { OperationType = OperationType.Buy },
            new Operations("sell", 2.00m, 5000) { OperationType = OperationType.Sell },
            new Operations("sell", 20.00m, 2000) { OperationType = OperationType.Sell },
            new Operations("sell", 20.00m, 2000) { OperationType = OperationType.Sell },
            new Operations("sell", 25.00m, 1000) { OperationType = OperationType.Sell },
            new Operations("buy", 20.00m, 10000) { OperationType = OperationType.Buy },
            new Operations("sell", 15.00m, 5000) { OperationType = OperationType.Sell },
            new Operations("sell", 30.00m, 4350) { OperationType = OperationType.Sell },
            new Operations("sell", 30.00m, 650) { OperationType = OperationType.Sell }
        };

        // Act
        var results = taxService.CalculateTaxes(operations);

        // Assert
        Assert.Equal(9, results.Count);
        Assert.Equal(0m, results[0].Tax);
        Assert.Equal(0m, results[1].Tax);
        Assert.Equal(0m, results[2].Tax);
        Assert.Equal(0m, results[3].Tax);
        Assert.Equal(3000.00m, results[4].Tax);
        Assert.Equal(0m, results[5].Tax);
        Assert.Equal(0m, results[6].Tax);
        Assert.Equal(3700.00m, results[7].Tax);
        Assert.Equal(0m, results[8].Tax);
    }
}
