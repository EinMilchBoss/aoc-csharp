using System.Diagnostics;

namespace Day02;

public readonly record struct Set(int R, int G, int B)
{
    public static Set FromString(string input)
    {
        int r = 0, g = 0, b = 0;
        foreach (var cubeString in input.Split(", "))
        {
            var (value, color) = Cube.FromString(cubeString);
            switch (color)
            {
                case "red":
                    r = value;
                    break;
                case "green":
                    g = value;
                    break;
                case "blue":
                    b = value;
                    break;
                default:
                    throw new UnreachableException("Cube has to be of color red, green or blue.");
            }
        }
        return new Set(r, g, b);
    }
}