using CapitalGains.Domain.Interfaces;
using CapitalGains.Domain.Models;
using System.Collections.ObjectModel;

namespace CapitalGains.Application.Services;

public class TaxService : ITaxService
{
    private readonly TaxCalculatorService _taxCalculator = new();

    public Collection<TaxResult> CalculateTaxes(Collection<Operations> operations)
    {
        if (operations == null) throw new ArgumentNullException(nameof(operations));

        decimal weightedAverage = 0m;
        int totalShares = 0;
        decimal accumulatedLoss = 0m;

        var results = new Collection<TaxResult>();

        foreach (var operation in operations)
        {
            var result = _taxCalculator.Calculate(operation, ref weightedAverage, ref totalShares, ref accumulatedLoss);
            results.Add(result);
        }

        return results;
    }
}