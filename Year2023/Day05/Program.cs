using System.Diagnostics;
using Util.Aoc;

var challenge = new Challenge(2023, 5);
var example = challenge.ReadInput("example.txt");
var actual = challenge.ReadInput("actual.txt");

var one = new Part<long>("Nearest location of seeds", PartOne);
var two = new Part<long>("Nearest location of seed ranges", PartTwo);

Console.WriteLine(one.Test(35, example));
Console.WriteLine(two.Test(46, example));

Console.WriteLine(one.Run(actual));
Console.WriteLine(two.Run(actual));

return;

long PartOne(string input)
{
    var blocks = input.Split($"{Environment.NewLine}{Environment.NewLine}");

    var seeds = ParseSeeds(blocks[0]);
    var almanac = Almanac.FromBlocks(blocks[1..]);

    return seeds.Min(almanac.Translate);
}

long PartTwo(string input)
{
    var blocks = input.Split($"{Environment.NewLine}{Environment.NewLine}");

    var seeds = ParseSeedIntervals(blocks[0]);
    var almanac = Almanac.FromBlocks(blocks[1..]);

    return seeds
        .SelectMany(almanac.Translate)
        .Min()!
        .Start;
}

IEnumerable<long> ParseSeeds(string line) =>
    line
        .Split(": ")[1]
        .Split(' ')
        .Select(long.Parse);

IEnumerable<Interval> ParseSeedIntervals(string line) =>
    ParseSeeds(line)
        .Chunk(2)
        .Select(pair => new Interval(pair[0], pair[0] + pair[1]));

class Almanac(IEnumerable<Map> maps)
{
    private readonly List<Map> _maps = [.. maps];

    public static Almanac FromBlocks(IEnumerable<string> blocks) =>
        new([.. blocks.Select(Map.FromBlock)]);

    public long Translate(long seed)
    {
        foreach (var map in _maps)
            seed = map.Translate(seed);

        return seed;
    }

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

    public Map(IEnumerable<TranslationInterval> intervals)
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

    public static Map FromBlock(string block)
    {
        var intervals = block.Split(Environment.NewLine)
            .Skip(1)
            .Select(TranslationInterval.FromLine)
            .ToList();

        var sortedIntervals = FillGaps(intervals);

        return new(sortedIntervals);
    }

    /// <summary>
    /// The list of intervals contains gaps where no translation occurs.
    /// This method adds artificial <see cref="TranslationInterval"/> with offset 0 to have all values from [0, long.MaxValue) covered by a <see cref="TranslationInterval"/>.
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

    public long Translate(long seed)
    {
        int i = BinarySearchIndex(seed);
        return seed + _sortedIntervals[i].Offset;
    }

    public IEnumerable<Interval> Translate(Interval source)
    {
        List<Interval> destinations = [];

        // To scan through all intervals we keep track of where we are.
        int i = BinarySearchIndex(source.Start);

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

    private int BinarySearchIndex(long value)
    {
        int start = 0, end = _sortedIntervals.Count - 1;
        while (start <= end)
        {
            int middle = start + (end - start) / 2;
            if (value < _sortedIntervals[middle].Start)
                end = middle - 1;
            else if (value >= _sortedIntervals[middle].End)
                start = middle + 1;
            else
                return middle;
        }

        throw new UnreachableException("Value could not be found in [0, long.MaxValue).");
    }
}

record TranslationInterval(long Start, long End, long Offset) :
    Interval(Start, End)
{
    public TranslationInterval(Interval interval, long offset = 0) :
        this(interval.Start, interval.End, offset)
    { }

    public static TranslationInterval FromLine(string line)
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
        ArgumentNullException.ThrowIfNull(other, nameof(other));

        return Start.CompareTo(other.Start);
    }

    public bool Contains(long value) =>
        Start <= value && value < End;
}