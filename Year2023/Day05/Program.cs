using System.Collections.Immutable;
using Day05;
using Util.Aoc;

var challenge = new Challenge(2023, 5);
var example = challenge.ReadInput("example.txt");
var actual = challenge.ReadInput("actual.txt");

// var one = new Part<uint>("Nearest location of seeds", PartOne);
var two = new Part<long>("Nearest location of seed ranges", PartTwo);

// Console.WriteLine(one.Test(35, example));
Console.WriteLine(two.Test(46, example));

// Console.WriteLine(one.Run(actual));
Console.WriteLine(two.Run(actual));

return;

/*uint PartOne(string input)
{
    var emptyLine = string.Join("", Enumerable.Repeat(Environment.NewLine, 2));
    var blocks = input.Split(emptyLine);
    var seeds = ParseSeeds(blocks.First());
    var almanac = Almanac.Parse(blocks.Skip(1));

    // Console.WriteLine($"seeds: {string.Join(' ', seeds)}");
    // Console.WriteLine();
    // Console.WriteLine(almanac);

    return seeds.Min(almanac.GetLocation);
}*/

IEnumerable<uint> ParseSeeds(string line)
{
    var seeds = line.Split(": ")[1];
    return seeds.Split(' ').Select(uint.Parse);
}

IEnumerable<Interval> ParseSeedIntervals(string line) =>
    line.Split(": ")[1]
        .Split(' ')
        .Select(long.Parse)
        .Chunk(2)
        .Select(pair => new Interval(pair[0], pair[0] + pair[1]));

long PartTwo(string input)
{
    string emptyLine = string.Join("", Enumerable.Repeat(Environment.NewLine, 2));
    string[] blocks = input.Split(emptyLine);

    var seeds = ParseSeedIntervals(blocks.First());
    var almanac = Almanac.FromBlocks(blocks.Skip(1));

    return seeds
        .SelectMany(almanac.Translate)
        .Min()!
        .Start;
}

class Almanac
{
    private readonly IEnumerable<Map> _maps;

    private Almanac(IEnumerable<Map> maps)
    {
        _maps = maps;
    }

    public static Almanac FromBlocks(IEnumerable<string> blocks) =>
        new
        ([
            ..blocks
            .Select(Map.Parse)
        ]);

    public IEnumerable<Interval> Translate(Interval interval)
    {
        List<Interval> intervals = [interval];
        foreach (var map in _maps)
        {
            List<Interval> dstIntervals = [];
            foreach (var srcInterval in intervals)
            {
                dstIntervals.AddRange(map.Translate(srcInterval));
            }

            intervals = dstIntervals;
        }

        return intervals;
    }
}

class Map
{
    private readonly List<TranslationInterval> _sortedIntervals;

    private Map(IEnumerable<TranslationInterval> intervals)
    {
        List<TranslationInterval> sortedIntervals = [.. intervals];

        if (!IsSorted(sortedIntervals))
            sortedIntervals.Sort();

        _sortedIntervals = sortedIntervals;
    }

    private static bool IsSorted(IEnumerable<TranslationInterval> enumerable)
    {
        long lastStart = enumerable.First().Start;
        foreach (var element in enumerable.Skip(1))
        {
            if (element.Start < lastStart)
                return false;

            lastStart = element.Start;
        }

        return true;
    }

    public static Map Parse(string block)
    {
        var intervals = block.Split(Environment.NewLine)
            .Skip(1)
            .Select(TranslationInterval.Parse)
            .ToList();

        var sortedIntervals = FillGaps(intervals);

        return new(sortedIntervals);
    }

    /// <summary>
    /// The list of intervals contains gaps where no translation occurs.
    /// This method adds artificial <see cref="TranslationInterval"/> with offset 0 to have all values from [0, long.MaxValue] covered by a <see cref="TranslationInterval"/>.
    /// </summary>
    /// <param name="intervals"></param>
    /// <returns></returns>
    private static List<TranslationInterval> FillGaps(List<TranslationInterval> intervals)
    {
        // We need to go through all intervals in ascending order.
        intervals.Sort();

        List<TranslationInterval> allIntervals = [];

        long nextStart = 0;
        for (int i = 0; i < intervals.Count; i++)
        {
            // Insert filling interval if the current interval doesn't connect to the previous interval.
            if (intervals[i].Start != nextStart)
                allIntervals.Add(new(nextStart, intervals[i].Start, 0));

            // Insert the current interval.
            allIntervals.Add(intervals[i]);

            // Keep track where the next interval is supposed to start.
            nextStart = intervals[i].End;
        }

        // If the last interval didn't cover the entire range yet, insert one last filling interval.
        if (nextStart != long.MaxValue)
            allIntervals.Add(new(nextStart, long.MaxValue, 0));

        return allIntervals;
    }

    public IEnumerable<Interval> Translate(Interval source)
    {
        List<Interval> destinations = [];

        // Scan through all intervals and keep track of where we are.
        int i = 0;

        // Find the first interval to start with.
        for (; i < _sortedIntervals.Count; i++)
        {
            if (_sortedIntervals[i].Contains(source.Start))
                break;
        }

        // The first interval could start in the middle of the i-th interval.
        // Therefore, we have to start with the source interval's start value.
        long start = source.Start;

        // Offset sub-intervals of input by entire intervals for as long as they are fully contained.
        for (; _sortedIntervals[i].End < source.End; i++)
        {
            destinations.Add(new(start + _sortedIntervals[i].Offset, _sortedIntervals[i].End + _sortedIntervals[i].Offset));

            start = _sortedIntervals[i].End;
        }

        // Add the last sub-interval that is not fully contained.
        // We only translate until the end of the source interval's end value.
        destinations.Add(new(start + _sortedIntervals[i].Offset, source.End + _sortedIntervals[i].Offset));

        return destinations;
    }
}

record TranslationInterval(long Start, long End, long Offset) :
    Interval(Start, End)
{
    public TranslationInterval(Interval interval, long offset = 0) :
        this(interval.Start, interval.End, offset)
    { }

    public static TranslationInterval Parse(string line)
    {
        string[] parts = line.Split(' ');
        var dstStart = long.Parse(parts[0]);
        var srcStart = long.Parse(parts[1]);
        var length = long.Parse(parts[2]);

        return new(Start: srcStart, End: srcStart + length, Offset: dstStart - srcStart);
    }
}

record Interval(long Start, long End) : IComparable<Interval>
{
    /// <summary>
    /// Intervals are sorted by their start value.
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