using Microsoft.Extensions.DependencyInjection;
using Vertical.CommandLine;
using Vertical.Scanner.Configuration;
using Vertical.Scanner.Execution;

var parserConfiguration = new ScanOptionsParserConfiguration();

parserConfiguration.OnExecute(options =>
{
    var services = ServicesFactory.CreateServices(options);
    var entryPoint = services.GetServices<IEntryPoint>().First(ep => ep.Handles());
    entryPoint.Execute();
});

try
{
    CommandLineApplication.Run(parserConfiguration, args);
}
catch (UsageException exception)
{
    Console.WriteLine(exception.Message);
    Console.WriteLine("Try scan --help");
}
