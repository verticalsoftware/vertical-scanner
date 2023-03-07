using Microsoft.Extensions.Logging;

namespace Vertical.Scanner.Configuration;

/// <summary>
/// Defines scan options.
/// </summary>
public class ScanOptions
{
    /// <summary>
    /// Gets the arguments after options.
    /// </summary>
    public List<string> Arguments { get; } = new();
    
    /// <summary>
    /// Gets or sets whether to print the version.
    /// </summary>
    public bool PrintVersion { get; set; }

    /// <summary>
    /// Gets or sets the log level.
    /// </summary>
    public LogLevel LogLevel { get; set; } = LogLevel.Error;
    
    /// <summary>
    /// Gets or sets whether to treat search strings as literal strings and not regular expressions.
    /// </summary>
    public bool FixedStrings { get; set; }

    /// <summary>
    /// Gets or sets whether to treat search strings as regular expression patterns.
    /// </summary>
    public bool BasicRegExpression { get; set; } = true;

    /// <summary>
    /// Gets the patterns to search for.
    /// </summary>
    public HashSet<string> Patterns { get; set; } = new();

    /// <summary>
    /// Gets the file paths or glob patterns to search for.
    /// </summary>
    public HashSet<string> FilePaths { get; set; } = new();

    /// <summary>
    /// Gets glob patterns to exclude from searches.
    /// </summary>
    public HashSet<string> FilePathsExcluded { get; set; } = new();

    /// <summary>
    /// Gets or sets whether to ignore case.
    /// </summary>
    public bool IgnoreCase { get; set; } = false;
    
    /// <summary>
    /// Gets or sets whether to not ignore case.
    /// </summary>
    public bool NoIgnoreCase { get; set; } = false;

    /// <summary>
    /// Gets or sets whether to invert the sense of matching.
    /// </summary>
    public bool InvertMatch { get; set; } = false;

    /// <summary>
    /// Gets or sets whether to match whole words.
    /// </summary>
    public bool WordMatches { get; set; } = false;

    /// <summary>
    /// Gets or sets whether to match whole lines.
    /// </summary>
    public bool LineMatches { get; set; } = false;

    /// <summary>
    /// Gets or sets whether to print byte offsets.
    /// </summary>
    public bool ByteOffsets { get; set; } = false;

    /// <summary>
    /// Gets or sets whether to print the matching file names, suppressing normal output.
    /// </summary>
    public bool FileMatchCount { get; set; } = false;

    /// <summary>
    /// Gets or sets when to colorize output.
    /// </summary>
    public ColorWhen ColorWhen { get; set; } = ColorWhen.Auto;

    /// <summary>
    /// Gets or sets whether to print paths of files that would not have produced matches.
    /// </summary>
    public bool FilesWithoutMatches { get; set; } = false;

    /// <summary>
    /// Gets or sets whether to print paths of files that produce matches.
    /// </summary>
    public bool FilesWithMatches { get; set; } = false;

    /// <summary>
    /// Gets or sets whether to print line numbers before match output.
    /// </summary>
    public bool LineNumbers { get; set; } = false;

    /// <summary>
    /// Gets or sets the max number of lines scan should match.
    /// </summary>
    public int MaxCount { get; set; } = -1;

    /// <summary>
    /// Gets or sets whether to only print matched, non-empty parts of a line.
    /// </summary>
    public bool OnlyMatching { get; set; } = false;

    /// <summary>
    /// Gets or sets quiet mode.
    /// </summary>
    public bool Quiet { get; set; } = false;

    /// <summary>
    /// Gets or sets whether to not print messages regarding I/O errors
    /// </summary>
    public bool NoMessages { get; set; } = false;
    
    /// <summary>
    /// Gets or sets whether to print all input content.
    /// </summary>
    public bool PrintAll { get; set; }
    
    /// <summary>
    /// Gets or sets whether to not print file paths in multi-file results.
    /// </summary>
    public bool NoPaths { get; set; }

    /// <summary>
    /// Gets or sets the surround spec.
    /// </summary>
    public string? SurroundSpec { get; set; }
    
    /// <summary>
    /// Gets or sets whether to print whitespace.
    /// </summary>
    public bool PreserveWhitespace { get; set; }
    
    /// <summary>
    /// Gets or sets the palette file.
    /// </summary>
    public string? TemplateFile { get; set; }
    
    /// <summary>
    /// Gets or sets the template configuration option
    /// </summary>
    public bool GetTemplates { get; set; }
    
    /// <summary>
    /// Gets or sets the template configuration option
    /// </summary>
    public string? SetTemplate { get; set; }
    
    /// <summary>
    /// Gets or sets where to write a template file.
    /// </summary>
    public string? TemplateOut { get; set; }
}