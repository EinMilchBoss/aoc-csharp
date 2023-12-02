namespace Day02;

public readonly record struct Cube(int Value, string Color)
{
    public static Cube FromString(string input)
    {
        var splitted = input.Split(" ");
        return new Cube(int.Parse(splitted[0]), splitted[1]);
    }
}