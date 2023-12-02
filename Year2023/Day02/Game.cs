namespace Day02;

public readonly record struct Game(int Id, Set[] Sets)
{
    private const int MAX_R = 12;
    private const int MAX_G = 13;
    private const int MAX_B = 14;

    public static Game FromString(string input)
    {
        var splitted = input.Split(": ");
        var id = int.Parse(splitted[0].Split(" ")[1]);
        var sets = splitted[1].Split("; ")
            .Select(Set.FromString)
            .ToArray();

        return new Game(id, sets);
    }

    public bool IsPossible() => Sets.All((set) => set.R <= MAX_R && set.G <= MAX_G && set.B <= MAX_B);

    public Set MinimalSet() => Sets.Aggregate((a, b) => new Set(Math.Max(a.R, b.R), Math.Max(a.G, b.G), Math.Max(a.B, b.B)));
}