using System.Collections.Immutable;

namespace Day05;

public readonly record struct Map(ImmutableArray<Range> Ranges)
{
    public static Map Parse(IEnumerable<string> lines)
    {
        var ranges = lines
            .Skip(1)
            .Select(Range.Parse)
            .ToImmutableArray();
        return new Map(ranges);
    }

    public override string ToString() => string.Join(Environment.NewLine, Ranges);
}