using System.Diagnostics;

namespace Util.Aoc;

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
