using Microsoft.Extensions.DependencyInjection;
using Vertical.Scanner.Configuration;

namespace Vertical.Scanner.Scanning;

public static class ScanningServiceExtensions
{
    public static IServiceCollection AddScanHandler(this IServiceCollection services, ScanOptions options)
    {
        switch (options)
        {
            case { FileMatchCount: true }:
                services.AddSingleton<IScanHandler, FileCountScanHandler>();
                break;
            
            case { FilesWithMatches: true }:
            case { FilesWithoutMatches: true }:
                services.AddSingleton<IScanHandler, FilePathScanHandler>();
                break;
            
            default:
                services.AddSingleton<IScanHandler, LineMatchingScanHandler>();
                break;
        }

        return services;
    }
}