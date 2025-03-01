using CapitalGains.Domain.Enums;
using CapitalGains.Infrastructure.IO;
using System.Text.Json;

namespace CapitalGains.Tests.UnitTests;

public class JsonInputParserTests
{
    [Fact]
    public void Parse_ValidJson_ReturnsCorrectOperations()
    {
        // Arrange
        var parser = new JsonInputParser();
        var jsonInput = @"[{""operation"":""buy"", ""unit-cost"":10.00, ""quantity"":10000}]";

        // Act
        var result = parser.Parse(jsonInput);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        var operation = result[0];
        Assert.Equal(OperationType.Buy, operation.OperationType);
        Assert.Equal("buy", operation.Operation);
        Assert.Equal(10.00m, operation.UnitCost);
        Assert.Equal(10000, operation.Quantity);
    }

    [Fact]
    public void Parse_EmptyJson_ThrowsJsonException()
    {
        // Arrange
        var parser = new JsonInputParser();
        var jsonInput = "[]";

        // Act & Assert
        Assert.Throws<JsonException>(() => parser.Parse(jsonInput));
    }

    [Fact]
    public void Parse_NullOrEmptyInput_ThrowsArgumentNullException()
    {
        // Arrange
        var parser = new JsonInputParser();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => parser.Parse(null));
        Assert.Throws<ArgumentNullException>(() => parser.Parse(""));
    }

    [Fact]
    public void Parse_InvalidOperationType_ThrowsArgumentException()
    {
        // Arrange
        var parser = new JsonInputParser();
        var jsonInput = @"[{""operation"":""invalid"", ""unit-cost"":10.00, ""quantity"":10000}]";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => parser.Parse(jsonInput));
    }
}
