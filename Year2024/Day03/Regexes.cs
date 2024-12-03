using System.Text.RegularExpressions;

namespace Day03;

public static partial class Regexes
{
    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
    public static partial Regex MulRegex();
    
    [GeneratedRegex(@"do\(\)")]
    public static partial Regex DoRegex();
    
    [GeneratedRegex(@"don\'t\(\)")]
    public static partial Regex DontRegex();
}