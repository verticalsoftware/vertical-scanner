namespace Vertical.Scanner.Matching;

public readonly ref struct MatchSection
{
    public MatchSection(MatchSectionType type, int matchId, ReadOnlySpan<char> value, int index, int length)
    {
        Type = type;
        MatchId = matchId;
        Value = value;
        Index = index;
        Length = length;
    }

    public MatchSectionType Type { get; }

    public int MatchId { get; }

    public ReadOnlySpan<char> Value { get; }

    public int Index { get; }

    public int Length { get; }
}    