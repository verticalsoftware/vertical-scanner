using Vertical.Scanner.Matching;

namespace Vertical.Scanner.Output;

public readonly record struct WriteDeferredSection(MatchSectionType Type, string Value);