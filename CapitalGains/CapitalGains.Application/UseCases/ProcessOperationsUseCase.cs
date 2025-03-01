using CapitalGains.Domain.Interfaces;
using CapitalGains.Domain.Models;
using System.Collections.ObjectModel;

namespace CapitalGains.Application.UseCases;

public class ProcessOperationsUseCase
{
    private readonly ITaxService _taxService;

    public ProcessOperationsUseCase(ITaxService taxService)
    {
        _taxService = taxService;
    }

    public Collection<TaxResult> Execute(Collection<Operations> operations)
    {
        if (operations == null || operations.Count == 0)
        {
            return new Collection<TaxResult>();
        }

        return _taxService.CalculateTaxes(operations);
    }
}