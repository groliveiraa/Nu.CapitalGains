using CapitalGains.Domain.Models;
using System.Collections.ObjectModel;

namespace CapitalGains.Domain.Interfaces;

public interface ITaxService
{
    Collection<TaxResult> CalculateTaxes(Collection<Operations> operations);
}
