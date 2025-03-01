using CapitalGains.Domain.Enums;
using CapitalGains.Domain.Models;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace CapitalGains.Infrastructure.IO;

public class JsonInputParser
{
    public Collection<Operations> Parse(string jsonInput)
    {
        if (string.IsNullOrEmpty(jsonInput))
        {
            throw new ArgumentNullException(nameof(jsonInput), "The JSON input cannot be null or empty.");
        }

        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var operations = JsonSerializer.Deserialize<Collection<Operations>>(jsonInput, options);

            if (operations == null || operations.Count == 0)
            {
                throw new JsonException("The operations list cannot be empty.");
            }

            var adjustedOperations = new Collection<Operations>();
            foreach (var operation in operations)
            {
                var operationType = operation.Operation.ToLower() switch
                {
                    "buy" => OperationType.Buy,
                    "sell" => OperationType.Sell,
                    _ => throw new ArgumentException($"Invalid operation type: {operation.Operation}")
                };

                var adjustedOperation = operation with { OperationType = operationType };
                adjustedOperations.Add(adjustedOperation);
            }

            return adjustedOperations;
        }
        catch (JsonException ex)
        {
            throw new JsonException($"Error parsing the input JSON: {ex.Message}", ex);
        }
    }
}
