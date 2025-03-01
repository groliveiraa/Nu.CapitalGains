using CapitalGains.Application.Services;
using CapitalGains.Application.UseCases;
using CapitalGains.Infrastructure.IO;
using System.Text.Json;

namespace CapitalGains.CLI;

class Program
{
    static void Main(string[] args)
    {
        var parser = new JsonInputParser();
        var processor = new ProcessOperationsUseCase(new TaxService());
        string line;

        while (!string.IsNullOrEmpty(line = Console.ReadLine()))
        {
            var operations = parser.Parse(line);
            var results = processor.Execute(operations);
            Console.WriteLine(JsonSerializer.Serialize(results));
        }
    }
}