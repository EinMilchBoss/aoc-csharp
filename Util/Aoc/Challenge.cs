namespace Util.Aoc;

public record Challenge(int year, int day)
{
    public string ReadInput(string filename)
    {
        var paddedDay = day.ToString().PadLeft(2, '0');
        return File.ReadAllText($"./res/Year{year}/Day{paddedDay}/{filename}");
    }
}