using System.Text.RegularExpressions;
using Shouldly;
using Vertical.Scanner.Matching;

namespace Vertical.Scanner.Tests.Matching;

public class RegexLineMatcherTests
{
    public record MatchResult(MatchSectionType SectionType, string Value);
    
    [Fact]
    public void MatchLine_Returns_Start_Sections()
    {
        var results = Collect("red green blue", "red");
        results.ShouldBe(new[]
        {
            new MatchResult(MatchSectionType.Match, "red"),
            new MatchResult(MatchSectionType.NonMatch, " green blue"),
        });
    }
    
    [Fact]
    public void MatchLine_Inverted_Returns_Start_Sections()
    {
        var results = Collect("red green blue", "red", invert: true);
        results.ShouldBe(new[]
        {
            new MatchResult(MatchSectionType.NonMatch, "red"),
            new MatchResult(MatchSectionType.Match, " green blue"),
        });
    }
    
    [Fact]
    public void MatchLine_Returns_Mid_Sections()
    {
        var results = Collect("red green blue", "green");
        results.ShouldBe(new[]
        {
            new MatchResult(MatchSectionType.NonMatch, "red "),
            new MatchResult(MatchSectionType.Match, "green"),
            new MatchResult(MatchSectionType.NonMatch, " blue")
        });
    }
    
    [Fact]
    public void MatchLine_Inverted_Returns_Mid_Sections()
    {
        var results = Collect("red green blue", "green", invert: true);
        results.ShouldBe(new[]
        {
            new MatchResult(MatchSectionType.Match, "red "),
            new MatchResult(MatchSectionType.NonMatch, "green"),
            new MatchResult(MatchSectionType.Match, " blue")
        });
    }
    
    [Fact]
    public void MatchLine_Returns_End_Sections()
    {
        var results = Collect("red green blue", "blue");
        results.ShouldBe(new[]
        {
            new MatchResult(MatchSectionType.NonMatch, "red green "),
            new MatchResult(MatchSectionType.Match, "blue")
        });
    }
    
    [Fact]
    public void MatchLine_Returns_Multiple_Sections()
    {
        var results = Collect("red green red blue red", "red");
        results.ShouldBe(new[]
        {
            new MatchResult(MatchSectionType.Match, "red"),
            new MatchResult(MatchSectionType.NonMatch, " green "),
            new MatchResult(MatchSectionType.Match, "red"),
            new MatchResult(MatchSectionType.NonMatch, " blue "),
            new MatchResult(MatchSectionType.Match, "red")
        });
    }
    
    [Fact]
    public void MatchLine_Inverted_Returns_End_Sections()
    {
        var results = Collect("red green blue", "blue", invert: true);
        results.ShouldBe(new[]
        {
            new MatchResult(MatchSectionType.Match, "red green "),
            new MatchResult(MatchSectionType.NonMatch, "blue")
        });
    }

    [Fact]
    public void MatchLine_Handles_Capture_Group()
    {
        var results = Collect("red green blue", "(green)");
        results.ShouldBe(new[]
        {
            new MatchResult(MatchSectionType.NonMatch, "red "),
            new MatchResult(MatchSectionType.Match, "green"),
            new MatchResult(MatchSectionType.NonMatch, " blue")
        });
    }
    
    [Fact]
    public void MatchLine_Inverted_Handles_Capture_Group()
    {
        var results = Collect("red green blue", "(green)", invert: true);
        results.ShouldBe(new[]
        {
            new MatchResult(MatchSectionType.Match, "red "),
            new MatchResult(MatchSectionType.NonMatch, "green"),
            new MatchResult(MatchSectionType.Match, " blue")
        });
    }

    [Fact]
    public void MatchLine_Word_Option_Handles_Start_Section_Capture_Group()
    {
        var results = Collect("word up", @"^(?:\W)?(word)|(word)(?:\W)?$");
        results.ShouldBe(new[]
        {
            new MatchResult(MatchSectionType.Match, "word"),
            new MatchResult(MatchSectionType.NonMatch, " up"),
        });
    }
    
    [Fact]
    public void MatchLine_Word_Option_Handles_End_Section_Capture_Group()
    {
        var results = Collect("up word", @"^(?:\W)?(word)|(word)(?:\W)?$");
        results.ShouldBe(new[]
        {
            new MatchResult(MatchSectionType.NonMatch, "up "),
            new MatchResult(MatchSectionType.Match, "word"),
        });
    }

    private IReadOnlyCollection<MatchResult> Collect(string input, string pattern, bool invert = false)
    {
        var results = new List<MatchResult>();
        new RegexLineMatcher(new Regex(pattern), inverted: invert).ScanLine(input, (in MatchSection section) => 
            results.Add(new MatchResult(section.Type, section.Value.ToString())));
        return results;
    }
}