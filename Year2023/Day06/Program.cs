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
        // Avoid "/ 2" by using ">> 1".
        long halfTime = (Time + 1) >> 1;

        // Always ignore the 0th iteration as it always yields 0.
        // We only iterate until we find the first match and derive the rest.
        long chargeTime = 1;
        while (chargeTime < halfTime)
        {
            if (ExceedsRecord(chargeTime))
                break;

            chargeTime++;
        }

        // Use "<< 1" instead of "* 2".
        long counter = (halfTime - chargeTime) << 1;

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
}