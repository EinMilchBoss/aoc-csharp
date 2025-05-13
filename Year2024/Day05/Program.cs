using System.Buffers;
using Util.Aoc;

var challenge = new Challenge(2024, 5);
var example = challenge.ReadInput("example.txt");
var actual = challenge.ReadInput("actual.txt");

var one = new Part<int>("N/A", PartOne);
var two = new Part<int>("N/A", PartTwo);

Console.WriteLine(one.Test(143, example));
// Console.WriteLine(two.Test(9, example));

Console.WriteLine(one.Run(actual));
// Console.WriteLine(two.Run(actual));

return;

int PartOne(string input)
{
    // Sum of middle page numbers of updates
    Console.WriteLine(input);

    var parts = input.Split("\n\n").Select(part => part.Split("\n")).ToArray();
    var rules = parts[0].Aggregate(new Dictionary<int, HashSet<int>>(), (acc, x) =>
    {
        var sides = x.Split("|");
        var key = int.Parse(sides[0]);
        var value = int.Parse(sides[1]);

        if (acc.TryGetValue(key, out var values))
        {
            values.Add(value);
        }
        else
        {
            acc.Add(key, [value]);
        }

        return acc;
    }).ToDictionary();
    var reversedRules = parts[0].Aggregate(new Dictionary<int, HashSet<int>>(), (acc, x) =>
    {
        var sides = x.Split("|");
        var value = int.Parse(sides[0]);
        var key = int.Parse(sides[1]);

        if (acc.TryGetValue(key, out var values))
        {
            values.Add(value);
        }
        else
        {
            acc.Add(key, [value]);
        }

        return acc;
    }).ToDictionary();
    var updates = parts[1].Select(x => x.Split(",").Select(int.Parse).ToArray()).ToArray();


    var sum = 0;
    foreach (var update in updates)
    {
        var isValid = true;
        for (var i = 0; i < update.Length && isValid; i++)
        {
            var rulesAfter = rules.GetValueOrDefault(update[i], []);
            var rulesBefore = reversedRules.GetValueOrDefault(update[i], []);

            for (var j = i + 1; j < update.Length; j++)
            {
                if (!rulesBefore.Contains(update[j])) continue;

                isValid = false;
                break;
            }

            for (var j = 0; j < i; j++)
            {
                if (!rulesAfter.Contains(update[j])) continue;

                isValid = false;
                break;
            }
        }

        if (isValid)
        {
            sum += update[update.Length / 2];
        }
    }

    return sum;
}

int PartTwo(string input)
{
    // values
    // values values


    return 0;
}

class Rule
{
    public HashSet<int> Values { get; private set; }
    public List<Rule> NextRules { get; private set; }

    public static Rule FromString(string[] ruleLines)
    {
        var dictionary = new Dictionary<int, Rule>();

        foreach (var ruleLine in ruleLines)
        {
            var ruleSides = ruleLine.Split("|");
            var key = int.Parse(ruleSides[0]);
            var value = int.Parse(ruleSides[1]);

            if (dictionary.TryGetValue(key, out var rule))
                rule.Values.Add(value);
            else
                dictionary.Add(key, new Rule
                {
                    Values = [value]
                });
        }

        // 1: 2, 3, 4
        // 3: 5, 6
        // 4: 5, 6

        // values: [2, 3, 4]
        // nextRules: [
        //   {
        //     values: [5, 6],
        //     nextRules: []
        //   },
        //   {
        //     values: [5, 6],
        //     nextRules: []
        //   }
        // ]

        var rules = new List<Rule>();

        while (dictionary.Count > 0)
        {
            var (currentKey, currentRule) = dictionary.First();

            foreach (var value in currentRule.Values)
            {
                if (dictionary.TryGetValue(value, out var rule))
                {
                    currentRule.NextRules.Add(rule);
                }
            }

            rules.Add(currentRule);
            dictionary.Remove(currentKey);
        }
    }
}
