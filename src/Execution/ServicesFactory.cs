using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vertical.Scanner.Configuration;
using Vertical.Scanner.Input;
using Vertical.Scanner.Matching;
using Vertical.Scanner.Output;
using Vertical.Scanner.Scanning;
using Vertical.Scanner.Templates;
using Vertical.SpectreLogger;
using Vertical.SpectreLogger.Options;

namespace Vertical.Scanner.Execution;

public static class ServicesFactory
{
    public static IServiceProvider CreateServices(ScanOptions options)
    {
        var services = new ServiceCollection();
        var loggerFactory = LoggerFactory.Create(configure => configure
            .SetMinimumLevel(options.LogLevel)
            .AddSpectreConsole(
                spectre =>
                {
                    spectre.SetMinimumLevel(options.NoMessages ? LogLevel.None : options.LogLevel);
                    spectre.UseSerilogConsoleStyle();
                    spectre.ConfigureProfiles(profile => profile.OutputTemplate = "[[{LogLevel}]] {Message}{NewLine}{Exception}");
                }));

        services.AddSingleton<IOptions<ScanOptions>>(new OptionsWrapper<ScanOptions>(options));
        services.AddSingleton(loggerFactory);
        services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        services.AddSingleton<IEntryPoint, VersionDisplay>();
        services.AddSingleton<IEntryPoint, TemplateConfiguration>();
        services.AddSingleton<IEntryPoint, ScannerImplementation>();
        services.AddSingleton<ILineMatcherFactory, LineMatcherFactory>();
        services.AddSingleton<ITemplateManager, TemplateManager>();
        services.AddSingleton<IOptions<TemplateOptions>, TemplateOptionsProvider>();
        services.AddSingleton<IOutputControllerFactory, OutputControllerFactory>();
        services.AddSourceInputs(options);
        services.AddScanHandler(options);
        services.AddOutputWriter(options);

        return services.BuildServiceProvider();
    }
}