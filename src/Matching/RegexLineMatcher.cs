using System.Text.RegularExpressions;

namespace Vertical.Scanner.Matching;

/// <summary>
/// Regular expression matcher.
/// </summary>
public class RegexLineMatcher : ILineMatcher
{
    private readonly Regex _regex;
    private readonly bool _inverted;

    /// <summary>
    /// Creates a new instance of this type
    /// </summary>
    public RegexLineMatcher(Regex regex, bool inverted)
    {
        _regex = regex;
        _inverted = inverted;
    }
    
    /// <inheritdoc />
    public int ScanLine(string input, MatchSectionCallback callback)
    {
        var callbacks = 0;
        var match = _regex.Match(input);
        var left = 0;

        while (match.Success)
        {
            for (var c = 0; c < match.Captures.Count; c++)
            {
                var capture = match.Captures[c];
                if (capture.Index > left)
                {
                    callback(new MatchSection(
                        GetMatchType(isMatch: false),
                        callbacks,
                        input[left..capture.Index],
                        left,
                        capture.Index - left));
                    callbacks++;
                }

                if (capture.Length > 0)
                {
                    callback(new MatchSection(
                        GetMatchType(isMatch: true),
                        callbacks,
                        capture.ValueSpan,
                        capture.Index,
                        capture.Length));
                    callbacks++;
                }

                left = capture.Index + capture.Length;
            }

            match = match.NextMatch();
        }

        if (left < input.Length)
        {
            callback(new MatchSection(
                GetMatchType(isMatch: false),
                callbacks,
                input[left..],
                left,
                input.Length - left));
            callbacks++;
        }

        return callbacks;
    }

    /// <inheritdoc />
    public bool IsMatch(string input)
    {
        var isMatch = _regex.IsMatch(input);
        return _inverted ? !isMatch : isMatch;
    }

    private MatchSectionType GetMatchType(bool isMatch)
    {
        isMatch = _inverted ? !isMatch : isMatch;
        return isMatch ? MatchSectionType.Match : MatchSectionType.NonMatch;
    }
}