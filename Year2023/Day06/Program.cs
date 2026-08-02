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

    return times
        .Zip(distances, (time, distance) => new Race(time, distance))
        .Aggregate(1L, (acc, race) => acc * race.CountBrokenRecordVariants());
}

long PartTwo(string input)
{
    string[][] split = SplitByLineAndWords(input);
    long times = long.Parse(string.Concat(split[0][1..]));
    long distances = long.Parse(string.Concat(split[1][1..]));

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
        // Exact lower and upper bounds using quadratic formula.
        double discriminant = Math.Sqrt((double)Time * Time - 4 * Record);
        double root1 = (Time - discriminant) / 2.0;
        double root2 = (Time + discriminant) / 2.0;

        // Calculate start and end of exceeding records.
        long first = (long)Math.Floor(root1) + 1;
        long last = (long)Math.Ceiling(root2) - 1;

        return last - first + 1;
    }
}