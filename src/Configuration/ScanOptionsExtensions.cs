using Vertical.CommandLine;

namespace Vertical.Scanner.Configuration;

public static class ScanOptionsExtensions
{
    /// <summary>
    /// Resolves inputs
    /// </summary>
    public static IEnumerable<string> GetResolvedFilePaths(this ScanOptions options)
    {
        return options switch
        {
            { Patterns.Count: > 0 } => options.Arguments,
            _ => options.Arguments.Skip(1)
        };
    }

    /// <summary>
    /// Resolves match patterns
    /// </summary>
    public static IEnumerable<string> GetMatchPatterns(this ScanOptions options)
    {
        return options switch
        {
            { Patterns.Count: 0 } => new[]{ options.Arguments.FirstOrDefault() ?? ".*" },
            _ => options.Patterns
        };
    }

    /// <summary>
    /// Resolves a surround region.
    /// </summary>
    public static SurroundRegion GetSurroundRegion(this ScanOptions options)
    {
        return SurroundRegion.Parse(options.SurroundSpec);
    }
}