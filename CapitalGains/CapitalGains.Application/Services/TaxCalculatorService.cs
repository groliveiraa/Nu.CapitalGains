using CapitalGains.Domain.Enums;
using CapitalGains.Domain.Models;

namespace CapitalGains.Application.Services;

public class TaxCalculatorService
{
    public TaxResult Calculate(Operations operation, ref decimal weightedAverage, ref int totalShares, ref decimal accumulatedLoss)
    {
        if (operation.OperationType == OperationType.Buy)
        {
            weightedAverage = ((weightedAverage * totalShares) + (operation.UnitCost * operation.Quantity)) / (totalShares + operation.Quantity);
            totalShares += operation.Quantity;
            return new TaxResult(0m);
        }
        else if (operation.OperationType == OperationType.Sell)
        {
            decimal operationValue = operation.UnitCost * operation.Quantity;
            decimal profitOrLoss = (operation.UnitCost - weightedAverage) * operation.Quantity;
            totalShares -= operation.Quantity;

            if (profitOrLoss > 0 && operationValue > 20000)
            {
                profitOrLoss -= accumulatedLoss;
                accumulatedLoss = profitOrLoss < 0 ? -profitOrLoss : 0;
                decimal tax = profitOrLoss > 0 ? Math.Round(profitOrLoss * 0.2m, 2) : 0m;
                return new TaxResult(tax);
            }
            else if (profitOrLoss < 0)
            {
                accumulatedLoss += Math.Abs(profitOrLoss);
            }
        }
        return new TaxResult(0m);
    }
}