using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vertical.Scanner.Configuration;

namespace Vertical.Scanner.Matching;

public class LineMatcherFactory : ILineMatcherFactory
{
    private readonly ILogger _logger;
    private readonly IOptions<ScanOptions> _options;
    private readonly char[] _escapeCharacters = @".$^{[(|)*+?\".ToCharArray();

    public LineMatcherFactory(ILogger<LineMatcherFactory> logger, IOptions<ScanOptions> options)
    {
        _logger = logger;
        _options = options;
    }

    /// <inheritdoc />
    public ILineMatcher CreateLineMatcher()
    {
        var split = _options.Value
            .GetMatchPatterns()
            .SelectMany(str => str.Split(@"\|", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            .Distinct();
        
        var patterns = split
            .Select(pattern => BuildRegularExpression(_options.Value, pattern))
            .ToArray();

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            foreach (var pattern in patterns)
            {
                _logger.LogDebug("Configuring match pattern '{Pattern}'", pattern);
            }
        }

        var compositePattern = string.Join('|', patterns);
        var options = _options.Value.IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;

        _logger.LogDebug("Setting options on regex matcher: {Options}", options);

        var regex = new Regex(compositePattern, options);
        return new RegexLineMatcher(regex, _options.Value.InvertMatch);
    }

    private string BuildRegularExpression(ScanOptions options, string pattern)
    {
        if (options.FixedStrings)
        {
            return Escape(pattern);
        }
         
        switch (options)
        {
            case { LineMatches: true }:
                return $"^{pattern}$";
            
            case { WordMatches: true }:
                return $@"^(?:\W)?{pattern}|{pattern}(?:\W)?$";
            
            default:
                return pattern;
        }
    }

    private string Escape(string input)
    {
        var sb = new StringBuilder(input.Length);
        foreach (var c in input)
        {
            if (_escapeCharacters.Contains(c))
            {
                sb.Append('\\');
            }
            
            sb.Append(c);
        }

        return sb.ToString();
    }
}