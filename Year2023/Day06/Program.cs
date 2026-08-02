using System.Net.Security;
using Util.Aoc;

var challenge = new Challenge(2023, 6);
var example = challenge.ReadInput("example.txt");
var actual = challenge.ReadInput("actual.txt");

var one = new Part<long>("Multiple small races", PartOne);
var two = new Part<long>("One big race", PartTwo);

Console.WriteLine(one.Test(288, example));
Console.WriteLine(two.Test(71503, example));

Console.WriteLine(one.Run(actual));
Console.WriteLine(two.Run(actual));

return;

long PartOne(string input)
{
    string[][] split = SplitByLineAndWords(input);
    IEnumerable<int> times = split[0][1..].Select(int.Parse);
    IEnumerable<int> distances = split[1][1..].Select(int.Parse);

    List<Race> races = [.. times.Zip(distances).Select(pair => new Race(pair.First, pair.Second))];

    return races.Aggregate(1L, (acc, race) => acc * race.CountBrokenRecordVariants());
}

long PartTwo(string input)
{
    string[][] split = SplitByLineAndWords(input);
    long times = long.Parse(string.Join(string.Empty, split[0][1..]));
    long distances = long.Parse(string.Join(string.Empty, split[1][1..]));

    Race race = new(times, distances);
    return race.CountBrokenRecordVariants();
}

string[][] SplitByLineAndWords(string input) =>
    [
        .. input
            .Split(Environment.NewLine)
            .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
    ];

readonly record struct Race(long Time, long Record)
{
    public long CountBrokenRecordVariants()
    {
        // Value was found out empirically.
        const int APPROXIMATION_ITERATIONS = 7;

        double approximation = ApproximateFirstBrokenRecord(APPROXIMATION_ITERATIONS);
        long firstBrokenRecord = (long)Math.Ceiling(approximation);

        // Avoid "/ 2" by using ">> 1".
        long halfTime = (Time + 1) >> 1;

        // We use the fact that the function is symmetrical.
        // Use "<< 1" instead of "* 2".
        long counter = (halfTime - firstBrokenRecord) << 1;

        // Make sure to count the middle value only once.
        // This is only relevant for even times (including 0, they are odd).
        if ((Time & 0b1) == 0b0 && ExceedsRecord(halfTime))
            counter += 1;

        return counter;
    }

    private bool ExceedsRecord(long chargeTime)
    {
        long travelTime = Time - chargeTime;
        return travelTime * chargeTime > Record;
    }

    /// <summary>
    /// Newton's method for approximating the root of our distance function.
    /// </summary>
    /// <param name="iterations">The amount of iterations for approximating the value.</param>
    /// <returns>Approximately, the first charge time to break the record of the race.</returns>
    private double ApproximateFirstBrokenRecord(int iterations)
    {
        double x = 0;
        for (int i = 0; i < iterations; i++)
            x -= F(x) / DF(x);
        return x;
    }

    /// <summary>
    /// f(x)<br/>
    /// = (t - x) * x - r<br/>
    /// = t*x - x^2 - r<br/>
    /// = -x^2 + t*x - r<br/>
    /// <br/>
    /// t := Time, r := Record
    /// </summary>
    /// <param name="x">The charge time.</param>
    /// <returns>The traveled distance.</returns>
    private double F(double x) =>
        -(x * x) + Time * x - Record;

    /// <summary>
    /// d/dx[f(x, t, d)]<br/>
    /// = -2x + t<br/>
    /// <br/>
    /// t := Time
    /// </summary>
    /// <param name="x">The charge time.</param>
    /// <returns></returns>
    private double DF(double x) =>
        -2 * x + Time;
}