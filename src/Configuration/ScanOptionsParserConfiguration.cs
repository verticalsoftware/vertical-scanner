using Microsoft.Extensions.Logging;
using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Help;

namespace Vertical.Scanner.Configuration;

/// <summary>
/// Represents the command line configuration.
/// </summary>
public class ScanOptionsParserConfiguration : ApplicationConfiguration<ScanOptions>
{
    /// <summary>
    /// Creates a new instance
    /// </summary>
    public ScanOptionsParserConfiguration()
    {
        Help.UseFile("README.md");
        HelpOption("--help", InteractiveConsoleHelpWriter.Default);

        Switch("-V|--version", arg => arg.Map.ToProperty(opt => opt.PrintVersion));
        Option<LogLevel>("--log-level", arg => arg.Map.ToProperty(opt => opt.LogLevel));
        
        // Pattern options
        Switch("-F|--fixed-strings", arg => arg.Map.ToProperty(opt => opt.FixedStrings));
        Switch("-G|--basic-regexp", arg => arg.Map.ToProperty(opt => opt.BasicRegExpression));
        
        // File options
        Option("-X", arg => arg.MapMany.ToCollection(opt => opt.FilePathsExcluded));
        
        // Match options
        Option("-e|--regexp", arg => arg.MapMany.ToCollection(opt => opt.Patterns));
        Option("-f|--file", arg => arg.MapMany.ToCollection(opt => opt.FilePaths));
        Switch("-i|--ignore-case", arg => arg.Map.ToProperty(opt => opt.IgnoreCase));
        Switch("--no-ignore-case", arg => arg.Map.ToProperty(opt => opt.NoIgnoreCase));
        Switch("-v|--invert-match", arg => arg.Map.ToProperty(opt => opt.InvertMatch));
        Switch("-w|--word-regexp", arg => arg.Map.ToProperty(opt => opt.WordMatches));
        Switch("-x|--line-regexp", arg => arg.Map.ToProperty(opt => opt.LineMatches));
        
        // Output options
        Switch("-b|--byte-offset", arg => arg.Map.ToProperty(opt => opt.ByteOffsets));
        Switch("-c|--count", arg => arg.Map.ToProperty(opt => opt.FileMatchCount));
        Option<ColorWhen>("--color", arg => arg.Map.ToProperty(opt => opt.ColorWhen));
        Switch("-L|--files-without-match", arg => arg.Map.ToProperty(opt => opt.FilesWithoutMatches));
        Switch("-l|--files-with-matches", arg => arg.Map.ToProperty(opt => opt.FilesWithMatches));
        Switch("--line-numbers", arg => arg.Map.ToProperty(opt => opt.LineNumbers));
        Switch("-o|--only-matches", arg => arg.Map.ToProperty(opt => opt.OnlyMatching));
        Switch("--no-paths", arg => arg.Map.ToProperty(opt => opt.NoPaths));
        Switch("-q|--quiet|--silent", arg => arg.Map.ToProperty(opt => opt.Quiet));
        Option<string?>("-p|--palette", arg => arg.Map.ToProperty(opt => opt.TemplateFile));
        Switch("-s|--no-messages", arg => arg.Map.ToProperty(opt => opt.NoMessages));
        Switch("-P|--print-all", arg => arg.Map.ToProperty(opt => opt.PrintAll));
        Option<string?>("-S|--surround", arg => arg.Map.ToProperty(opt => opt.SurroundSpec));
        Switch("-W|--preserve-whitespace", arg => arg.Map.ToProperty(opt => opt.PreserveWhitespace));
        
        // Configuration
        Switch("--get-templates", arg => arg.Map.ToProperty(opt => opt.GetTemplates));
        Option<string?>("--set-template", arg => arg.Map.ToProperty(opt => opt.SetTemplate));
        Option<string?>("--template-out", arg => arg.Map.ToProperty(opt => opt.TemplateOut));
     
        // Arguments
        PositionArgument(arg => arg.MapMany.ToCollection(opt => opt.Arguments));
    }
}