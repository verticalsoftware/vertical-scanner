Vertical Scanner - a cross-platform 'grep' like utility.

USAGE    
    scan [OPTIONS] <PATTERNS> [FILE...]
    
DESCRIPTION
    You'll find that this utility attempts to get as close to grep as possible.
    
QUICK REFERENCE
    Simple search of file:              scan findme ./text.txt
    Directory search:                   scan findme ./**/*
    Directory search of txt files:      scan findme ./**/*.txt
    
ARGUMENTS
    <PATTERNS>
        One or more search patterns to match within lines of source input, each separated by an escaped pipe character. If the search includes spaces, enclose the search patterns in double quotes. To specify multiple search patterns as separate arguments, use the -e or --regexp option. If -e or --regexp option is used, the scanner will treat all positional parameters as file path inputs.
        
    [FILE...]
        One or more paths to files or glob patterns to match multiple files. Multiple globs can be represented in a single argument by separating entries by a pipe character.

GENERIC OPTIONS
    --help
        Print this document
        
    --log-level <trace|debug|information|warning|error|critical|none>
        Configures the internal logger, defaults to error (overriden to none when using -s or --no-messages)

    -V, --version
        Print the utility version

PATTERN OPTIONS
    -F, --fixed-strings
        Do not treat PATTERNS as a regular expression. Specifying this option causes all regular expression related options (-w, --word-regexp, -x, --line-regexp) to have no effect.
        
    -G, --basic-regexp
        Treats PATTERNS as regular expressions as implemented by the .Net regular expression engine. This is the default.
        
FILE OPTIONS
    -X <glob>
        Exclude <glob> from any file pattern matches. This option may be repeated multiple times or concatenated in a single value by separating entries with the pipe character.                 

MATCH OPTIONS
    -e <PATTERNS>, --regexp <PATTERNS>
        Uses PATTERNS as the patterns matched to input sources. Use this option if the search pattern begins with "-". This option can be used multiple times, and is the recommended way to specify multiple patterns.
        Note - if this option is used, then the scanner treats all position-based arguments as FILE parameters.  
        
    -f <PATH>, --file <PATH>
        Use patterns defined in a file, one per line. This option can be used multiple times.

    -i, --ignore-case
        Ignores case when matching input patterns to match patterns. Modifies the regular expression options or uses .Net case-insensitive string comparers.
        
    --no-ignore-case
        Evaluates case when matching input patterns to match patterns. This is the default.
        
    -v, --invert-match
        Selects lines from input sources that do not match PATTERNS.
        
    -w, --word-regexp
        Modifies the search pattern such that the resulting regular expression selects lines containing matches to a whole word to the beginning or end of the scan line, or preceded or followed by a non-word constituent character.
        
    -x, --line-regexp
        Modifies the search pattern such that the resulting regular expression matches the entire scan line.
        
OUTPUT OPTIONS
    -b, --byte-offset
        Print the byte offset of the input file before each output line.
        
    -c, --count
        Replace normal output with the number of matched files.
        
    --color [ALWAYS|NEVER|AUTO]
        Print matches such that ansi control characters are included that can colorize matched sections of the input line. Colors used can be defined in a palette file. This option is set by default to 'AUTO'.          

    -L, --files-without-match                                     
        Replace normal output with the paths of files that would have not produced output.
        
    -l, --files-with-matches
        Replace normal output with the paths of files that would have produced output.
        
    --line-numbers
        Print the line number before each output line. This option has no effect when other options that suppress normal output are used.        
        
    -m <COUNT>, --max <COUNT>
        Indicates reading of file should stop after <count> matching lines are encountered. If <count> is zero, the file is not read. -1 is considered infinity.
        
    --no-paths
        Do not print file paths.        
        
    -o, --only-matching
        Print only matched, non-empty parts of a matching line. This option is ignored when using surrounding regions (-S, --surround).

    -q, --quiet, --silent
        Do not print any results and exit with a zero return code if any match is found.
        
    --palette <PATH>
        Path to a palette file. By default the scanner will look in $USERPROFILE/.vertical-scanner/palette.txt.
        
    -P, --print-all
        Prints all lines of input sources regardless of whether or not matches are made.    
        
    -s, --no-messages
        Ignore error messages relating to I/O operations.
        
    -S, --surround <[SPEC]>
        Surround matching input lines with non-matching input lines that occur before and/or after the matched line. -o has no effect on printing non-matching lines. This option is not available when using other options that modify normal output: -C, --count, -l, --files-with-matches, -q, --quiet, --silent.
        Examples:
            scan --surround=-5      Prints up to 5 lines that are scanned before the matched line
            scan --surround=+5      Prints up to 5 lines that are scanned after the matched line
            scan --surround=5       Prints up to 5 lines that are scanned before and after the matched line
            
CONFIGURATION OPTIONS
    --get-templates
        Gets the configured render templates.
    
    --set-template [ID=TEMPLATE]
        Modifies the user's local palette file by adding or updating a key/value pair.
        ID
            The ID of the template to update. Can be one of the following:
                MatchingSubstrings<N>
                NonMatchingSubstrings
                ByteOffsets
                LineNumbers
                FileInfo
            
            <N> of MatchingSubstrings is a zero-based index. The scanner can colorize distinct values using different colors. The templates defined here will form a pool of templates that scanner will pick from in order. There cannot be any gaps in the indices of the keys for this template type.
            
        TEMPLATE
            The template used to render the value. Use SpectreConsole markup along with a single parameter placeholder to specify a template.
            
        Examples:
            scan --set-palette-color MatchingSubstrings0=[green]{0}[/]  - Renders the first distinct matched value in green
            scan --set-palette-color MatchingSubstrings1=[red]{0}[/]  - Renders the second distinct matched value in red...
            scan --set-palette-color ByteOffsets=[pink italic]{0}[/]    - Renders byte offsets in italicized pink                            
    
    --template-out [PATH]
        Write the modified template file to [PATH]. Used in combination with --set-template.                              