namespace Util.Aoc;

public record Challenge(int year, int day)
{
    public string ReadInput(string filename)
    {
        var paddedDay = day.ToString().PadLeft(2, '0');
        // Probably not the best idea, but this lets me debug in peace at least.
        return File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}/../../../../../res/Year{year}/Day{paddedDay}/{filename}");
    }
}