using System.Collections.Immutable;
using System.Diagnostics;

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

    public uint Convert(uint id)
    {
        foreach (var range in Ranges)
        {
            if (range.ContainsSource(id))
                return id - range.SourceStart + range.DestinationStart;
        }
        return id;
    }

    public override string ToString() => string.Join(Environment.NewLine, Ranges);
}