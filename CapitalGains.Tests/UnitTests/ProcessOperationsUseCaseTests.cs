using CapitalGains.Application.UseCases;
using CapitalGains.Domain.Enums;
using CapitalGains.Domain.Interfaces;
using CapitalGains.Domain.Models;
using System.Collections.ObjectModel;

namespace CapitalGains.Tests.UnitTests;

public class ProcessOperationsUseCaseTests
{
    [Fact]
    public void Execute_ValidOperations_CallsTaxServiceAndReturnsResults()
    {
        // Arrange
        var taxServiceMock = new MockTaxService();
        var useCase = new ProcessOperationsUseCase(taxServiceMock);
        var operations = new Collection<Operations>
            {
                new Operations("buy", 10.00m, 10000) { OperationType = OperationType.Buy }
            };

        // Act
        var results = useCase.Execute(operations);

        // Assert
        Assert.NotNull(results);
        Assert.Single(results);
        Assert.True(taxServiceMock.CalculateTaxesCalled);
    }

    [Fact]
    public void Execute_NullOperations_ReturnsEmptyCollection()
    {
        // Arrange
        var taxServiceMock = new MockTaxService();
        var useCase = new ProcessOperationsUseCase(taxServiceMock);

        // Act
        var results = useCase.Execute(null);

        // Assert
        Assert.NotNull(results);
        Assert.Empty(results);
        Assert.False(taxServiceMock.CalculateTaxesCalled);
    }
}

// Mock para ITaxService
public class MockTaxService : ITaxService
{
    public bool CalculateTaxesCalled { get; private set; }

    public Collection<TaxResult> CalculateTaxes(Collection<Operations> operations)
    {
        CalculateTaxesCalled = true;
        return new Collection<TaxResult> { new TaxResult(0m) };
    }
}