using System.Text.RegularExpressions;

using Util.Aoc;

using Day03;

var challenge = new Challenge(2024, 3);
var example1 = challenge.ReadInput("example1.txt");
var example2 = challenge.ReadInput("example2.txt");
var actual = challenge.ReadInput("actual.txt");

var one = new Part<int>("Sum of products", PartOne);
var two = new Part<int>("Sum of enabled products", PartTwo);

Console.WriteLine(one.Test(161, example1));
Console.WriteLine(two.Test(48, example2));

Console.WriteLine(one.Run(actual));
Console.WriteLine(two.Run(actual));

return;

int PartOne(string input)
{
    var matches = Regexes.MulRegex().Matches(input);
    
    return matches.Sum(GetProduct);
}

int PartTwo(string input)
{
    var products = GetIndexedProducts(Regexes.MulRegex().Matches(input));
    var doIndices = GetMatchIndices(Regexes.DoRegex().Matches(input));
    var dontIndices = GetMatchIndices(Regexes.DontRegex().Matches(input));

    var sum = 0;
    var isEnabled = true;
    for (var i = 0; i < input.Length; i++)
    {
        if (doIndices.Contains(i))
            isEnabled = true;
        
        if (dontIndices.Contains(i))
            isEnabled = false;

        if (isEnabled)
            sum += products.GetValueOrDefault(i);
    }
    
    return sum;
}

int GetProduct(Match match)
{
    // We need to skip the first element because for some reason that is the match itself.
    return match.Groups
        .Skip<Group>(1)
        .Select(group => int.Parse(group.Value))
        .Aggregate((acc, x) => acc * x);
}

Dictionary<int, int> GetIndexedProducts(MatchCollection matches)
{
    return matches
        .Select(GetGroupIndexProductPairOfMatch)
        .ToDictionary();
}

(int index, int product) GetGroupIndexProductPairOfMatch(Match match)
{
    var pairs = GetGroupIndexValuePairsOfMatch(match);

    var first = int.Parse(pairs[0].value);
    var second = int.Parse(pairs[1].value);
    
    return (pairs[0].index, first * second);
}

(int index, string value)[] GetGroupIndexValuePairsOfMatch(Match match)
{
    // We need to skip the first element because for some reason that is the match itself.
    return match.Groups
        .Skip<Group>(1)
        .Select(group => (group.Index, group.Value))
        .ToArray();
}
    
HashSet<int> GetMatchIndices(MatchCollection matches)
{
    return matches
        .Select(match => match.Index)
        .ToHashSet();
}