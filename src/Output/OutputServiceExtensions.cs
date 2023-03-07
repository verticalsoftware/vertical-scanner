using Microsoft.Extensions.DependencyInjection;
using Vertical.Scanner.Configuration;

namespace Vertical.Scanner.Output;

public static class OutputServiceExtensions
{
    public static IServiceCollection AddOutputWriter(this IServiceCollection services, ScanOptions options)
    {
        switch (options.ColorWhen)
        {
            case ColorWhen.Always:
            case ColorWhen.Auto:
                services.AddSingleton<IOutputWriter, ColorizingOutputWriter>();
                break;
            
            default:
                break;
        }

        return services;
    }
}