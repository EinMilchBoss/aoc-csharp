using System.Diagnostics;

namespace Util.Aoc;

public class Part<T> where T : struct
{
    private readonly string _name;
    private readonly Solve<T> _solve;

    public Part(string name, Solve<T> solve)
    {
        _name = name;
        _solve = solve;
    }

    public string Test(T expected, string input)
    {
        var actual = _solve(input);
        return EqualityComparer<T>.Default.Equals(actual, expected) 
            ? $"[TEST] {_name}: PASS" 
            : $"[TEST] {_name}: FAIL (expected: {expected}, found {actual})";
    }

    public string Run(string input)
    {
        var watch = Stopwatch.StartNew();
        var result = _solve(input);
        watch.Stop();

        var nanosecondsPerTick = 1_000L * 1_000L * 1_000L / Stopwatch.Frequency;
        var deltaInMicroseconds = watch.ElapsedTicks / nanosecondsPerTick / 1_000;
        return $"""
        {_name} ({deltaInMicroseconds} µs):
        {result}
        """;
    }
}
