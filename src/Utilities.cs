namespace Vertical.Scanner;

public static class Utilities
{
    public static IEnumerable<string> Split(IEnumerable<string> input, char c)
    {
        return input
            .SelectMany(str => str.Split(c))
            .Select(str => str.Trim())
            .Where(str => !string.IsNullOrWhiteSpace(str))
            .Distinct();
    }

    public static IEnumerable<string> PipeSplit(IEnumerable<string> input) => Split(input, '|');
}