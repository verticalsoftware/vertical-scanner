using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vertical.Scanner.Configuration;

namespace Vertical.Scanner.Input;

public static class InputServiceExtensions
{
    public static IServiceCollection AddSourceInputs(
        this IServiceCollection services,
        ScanOptions options)
    {
        var excludePatterns = Utilities.PipeSplit(options.FilePathsExcluded).ToArray();
        
        foreach (var path in options.GetResolvedFilePaths())
        {
            if (!path.Contains('*'))
            {
                AddFileSourceInput(services, path, options.ByteOffsets);
                continue;
            }

            var matcher = new Matcher();
            foreach (var pattern in excludePatterns)
            {
                matcher.AddExclude(pattern);
            }

            foreach (var pattern in Utilities.PipeSplit(options.GetResolvedFilePaths()))
            {
                matcher.AddInclude(pattern);
            }

            foreach (var match in matcher.GetResultsInFullPath(Directory.GetCurrentDirectory()))
            {
                AddFileSourceInput(services, match, options.ByteOffsets);
            }
        }

        return services;
    }

    private static void AddFileSourceInput(IServiceCollection services, string path, bool trackBytes)
    {
        services.AddSingleton<ISourceInput>(sp => new FileSourceInput(
            sp.GetRequiredService<IOptions<ScanOptions>>(),
            sp.GetRequiredService<ILogger<FileSourceInput>>(),
            path,
            trackBytes));
    }
}