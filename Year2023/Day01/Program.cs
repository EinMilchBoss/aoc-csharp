using System.Diagnostics;

public record Challenge(int year, int day)
{
    public string ReadInput(string filename)
    {
        var paddedDay = day.ToString().PadLeft(2, '0');
        return File.ReadAllText($"./res/Year{year}/Day{paddedDay}/{filename}");
    }
}

public delegate T Solve<T>(string input);

public class Part<T> where T : struct
{
    private string name;
    private Solve<T> solve;

    public Part(string name, Solve<T> solve)
    {
        this.name = name;
        this.solve = solve;
    }

    public string Test(T expected, string input)
    {
        var actual = solve(input);
        if (EqualityComparer<T>.Default.Equals(actual, expected))
        {
            return $"[TEST] Part {name}: PASS";
        }
        else
        {
            return $"[TEST] Part {name}: FAIL (expected: {expected}, found {actual})";
        }
    }

    public string Run(string input)
    {
        var watch = Stopwatch.StartNew();
        var result = solve(input);
        watch.Stop();

        var nanosecondsPerTick = 1_000L * 1_000L * 1_000L / Stopwatch.Frequency;
        var deltaInMicroseconds = watch.ElapsedTicks / nanosecondsPerTick / 1_000.0;
        return $"""
        {name} ({deltaInMicroseconds} µs):
        {result}
        """;
    }
}

public static class Program
{
    public static void Main()
    {
        var challenge = new Challenge(2023, 1);
        var example1 = challenge.ReadInput("example1.txt");
        var example2 = challenge.ReadInput("example2.txt");
        var actual = challenge.ReadInput("actual.txt");

        var one = new Part<int>("Calibration sum", PartOne);
        var two = new Part<int>("Advanced calibration sum", PartTwo);

        Console.WriteLine(one.Test(142, example1));
        Console.WriteLine(two.Test(281, example2));

        Console.WriteLine(one.Run(actual));
        Console.WriteLine(two.Run(actual));
    }

    public static int PartOne(string input)
    {
        var sum = 0;
        var lines = input.Split(Environment.NewLine);
        foreach (var line in lines)
        {
            int first = 0, last = 0;
            for (var i = 0; i < line.Length; i += 1)
            {
                if (char.IsDigit(line[i]))
                {
                    first = line[i] - '0';
                    break;
                }
            }
            for (var i = line.Length - 1; i >= 0; i -= 1)
            {
                if (char.IsDigit(line[i]))
                {
                    last = line[i] - '0';
                    break;
                }
            }
            sum += first * 10 + last;
        }

        return sum;
    }

    public static int PartTwo(string input)
    {
        var numberWords = new Dictionary<string, int>()
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3,
            ["four"] = 4,
            ["five"] = 5,
            ["six"] = 6,
            ["seven"] = 7,
            ["eight"] = 8,
            ["nine"] = 9,
        };

        var sum = 0;
        var lines = input.Split(Environment.NewLine);
        foreach (var line in lines)
        {
            int first = 0, last = 0;
            int firstIndex = line.Length - 1, lastIndex = 0;

            // durchsuche nach wörtern
            for (var i = 0; i < line.Length; i += 1)
            {
                foreach (var word in numberWords.Keys)
                {
                    //Console.WriteLine($"{i} {word}");
                    if (line.Substring(i).StartsWith(word))
                    {
                        first = numberWords[word];
                        firstIndex = i;
                        goto FoundFirstWord;
                    }
                }
            }
        FoundFirstWord:;

            for (var i = line.Length - 1; i >= 0; i -= 1)
            {
                foreach (var word in numberWords.Keys)
                {
                    if (line.Substring(i).StartsWith(word))
                    {
                        last = numberWords[word];
                        lastIndex = i;
                        goto FoundLastWord;
                    }
                }
            }
        FoundLastWord:;

            // durchsuchen nach zahlen
            for (var i = 0; i <= firstIndex; i += 1)
            {
                if (char.IsDigit(line[i]))
                {
                    first = line[i] - '0';
                    break;
                }
            }

            for (var i = line.Length - 1; i >= lastIndex; i -= 1)
            {
                if (char.IsDigit(line[i]))
                {
                    last = line[i] - '0';
                    break;
                }
            }
            //Console.WriteLine(first * 10 + last);

            sum += first * 10 + last;
        }


        return sum;
    }
}