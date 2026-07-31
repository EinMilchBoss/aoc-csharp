using System.Collections.Generic;
using System.Collections.Immutable;
using Day05;
using Util.Aoc;

var challenge = new Challenge(2023, 5);
var example = challenge.ReadInput("example.txt");
var actual = challenge.ReadInput("actual.txt");

var one = new Part<uint>("Nearest location of seeds", PartOne);
var two = new Part<long>("Nearest location of seed ranges", PartTwo);

Console.WriteLine(one.Test(35, example));
Console.WriteLine(two.Test(46, example));

Console.WriteLine(one.Run(actual));
Console.WriteLine(two.Run(actual));

return;

uint PartOne(string input)
{
    var emptyLine = string.Join("", Enumerable.Repeat(Environment.NewLine, 2));
    var blocks = input.Split(emptyLine);
    var seeds = ParseSeeds(blocks.First());
    var almanac = Almanac.Parse(blocks.Skip(1));

    // Console.WriteLine($"seeds: {string.Join(' ', seeds)}");
    // Console.WriteLine();
    // Console.WriteLine(almanac);

    return seeds.Min(almanac.GetLocation);
}

IEnumerable<uint> ParseSeeds(string line)
{
    var seeds = line.Split(": ")[1];
    return seeds.Split(' ').Select(uint.Parse);
}

IEnumerable<Interval> ParseSeedRanges(string line)
{
    var seeds = line.Split(": ")[1];
    var values = seeds
        .Split(' ')
        .Select(uint.Parse)
        .ToImmutableArray();

    var seedIntervals = new List<Interval>();
    for (int i = 0; i < values.Length; i += 2)
        seedIntervals.Add(new Interval(values[i], values[i] + values[i + 1]));

    return seedIntervals.AsEnumerable();
}

long PartTwo(string input)
{
    var emptyLine = string.Join("", Enumerable.Repeat(Environment.NewLine, 2));
    var blocks = input.Split(emptyLine);
    var seeds = ParseSeedRanges(blocks.First());

    var maps = blocks.Skip(1).Select(Map.Parse).ToArray();

    foreach (var m in maps)
    {
        var next = new List<Interval>();
        foreach (var s in seeds)
        {
            next.AddRange(m.Translate(s));
        }
        seeds = next;
    }

    return seeds.Min()!.Start;
}

class Map
{
    private readonly IList<TranslationInterval> _sortedIntervals;

    private Map(IList<TranslationInterval> sortedIntervals)
    {
        _sortedIntervals = sortedIntervals;
    }

    public static Map Parse(string block)
    {
        var intervals = block.Split(Environment.NewLine)
            .Skip(1)
            .Select(TranslationInterval.Parse)
            .ToList();

        var sortedIntervals = Fill(intervals);

        return new(sortedIntervals);
    }

    private static List<TranslationInterval> Fill(List<TranslationInterval> intervals)
    {
        var ret = new List<TranslationInterval>();

        intervals.Sort();

        long next = 0;
        for (int i = 0; i < intervals.Count; i++)
        {
            if (intervals[i].Interval.Start != next)
            {
                // Insert without offset.
                ret.Add(new TranslationInterval(new Interval(next, intervals[i].Interval.Start), 0));
            }

            // Insert with offset.
            ret.Add(intervals[i]);
            next = intervals[i].Interval.End;
        }

        // Insert remaining if necessary
        if (next != uint.MaxValue)
        {
            ret.Add(new TranslationInterval(new Interval(next, long.MaxValue), 0));
        }


        return ret;
    }

    public List<Interval> Translate(Interval src)
    {
        // Scan through all intervals and keep track of where you are.
        int i = 0;

        // Find the first interval to start with.
        for (; i < _sortedIntervals.Count; i++)
        {
            if (_sortedIntervals[i].Contains(src.Start))
                break;
        }

        var ret = new List<Interval>();
        long start = src.Start;
        long end = _sortedIntervals[i].Interval.End;
        while (src.End > end)
        {
            ret.Add(new Interval(start + _sortedIntervals[i].Offset, end + _sortedIntervals[i].Offset));

            i++;
            start = _sortedIntervals[i].Interval.Start;
            end = _sortedIntervals[i].Interval.End;
        }

        ret.Add(new Interval(start + _sortedIntervals[i].Offset, src.End + _sortedIntervals[i].Offset));

        return ret;
    }
}

record TranslationInterval(Interval Interval, long Offset) : IComparable<TranslationInterval>
{
    public static TranslationInterval Parse(string line)
    {
        string[] parts = line.Split(' ');
        var dstStart = long.Parse(parts[0]);
        var srcStart = long.Parse(parts[1]);
        var length = long.Parse(parts[2]);

        var interval = new Interval(srcStart, checked(srcStart + length));
        long offset = checked(dstStart - srcStart);
        return new(interval, offset);
    }

    /// <summary>
    /// Translation intervals are soly sorted by their intervals' start value.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(TranslationInterval? other)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(other));

        return Interval.CompareTo(other!.Interval);
    }

    public bool Contains(long value) =>
        Interval.Contains(value);
}

record Interval(long Start, long End) : IComparable<Interval>
{
    /// <summary>
    /// Intervals are sorted soley by their start value.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Interval? other)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(other));

        return Start.CompareTo(other!.Start);
    }

    public bool Contains(long value) =>
        Start <= value && value < End;
}