using CapitalGains.Domain.Enums;
using System.Text.Json.Serialization;

namespace CapitalGains.Domain.Models;

public record Operations(
    [property: JsonPropertyName("operation")] string Operation,
    [property: JsonPropertyName("unit-cost")] decimal UnitCost,
    [property: JsonPropertyName("quantity")] int Quantity)
{
    public OperationType OperationType { get; init; }
}