using System.Text.RegularExpressions;

namespace Vertical.Scanner.Configuration;

public class SurroundRegion
{
    private SurroundRegion(int countBefore, int countAfter)
    {
        CountBefore = countBefore;
        CountAfter = countAfter;
    }

    public int CountBefore { get; }

    public int CountAfter { get; }

    public static SurroundRegion Parse(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new SurroundRegion(0, 0);

        try
        {
            var match = Regex.Match(value, @"([+\-])?(\d+)?");
            var control = match.Groups[1].Value;
            var count = int.Parse(match.Groups[2].Value); 
            
            switch (control)
            {
                case "-":
                    return new SurroundRegion(count, 0);
                case "+":
                    return new SurroundRegion(0, count);
                case "":
                    return new SurroundRegion(count, count);
            }
        }
        catch
        {
            // Throw later
        }
        
        throw new ApplicationException($"Invalid surround parameter '{value}'");
    }
}