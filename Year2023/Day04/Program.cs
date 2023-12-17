using Day04;
using Util.Aoc;

var challenge = new Challenge(2023, 4);
var example = challenge.ReadInput("example.txt");
var actual = challenge.ReadInput("actual.txt");

var one = new Part<int>("Sum of card points", PartOne);
var two = new Part<int>("Amount of total cards won", PartTwo);

Console.WriteLine(one.Test(13, example));
Console.WriteLine(two.Test(30, example));

Console.WriteLine(one.Run(actual));
Console.WriteLine(two.Run(actual));

int PartOne(string input) => input
    .Split(Environment.NewLine)
    .Select((line) => Card.Parse(line).Points())
    .Sum();

int PartTwo(string input)
{
    var parsed = input
        .Split(Environment.NewLine)
        .Select(Card.Parse);

    var totalAmount = TotalAmountOfCards(parsed);
    return totalAmount;
}

int TotalAmountOfCards(IEnumerable<Card> cards)
{
    // pop first number m from list
    // get n matches of current card
    // next n numbers in list " + m"
    // add m to score

    int Iterate(Queue<Card> cards, List<int> cardAmounts, int score)
    {
        if (!cards.Any())
            return score;

        var card = cards.Dequeue();
        var cardAmount = cardAmounts.First();
        var matches = card.GetMatches();

        var remainingAmounts = new List<int>(cardAmounts.Skip(1));
        for (var i = 0; i < matches; i += 1)
        {
            remainingAmounts[i] += cardAmount;
        }

        //Console.WriteLine(cardAmount);

        return Iterate(cards, remainingAmounts, score + cardAmount);
    }

    return Iterate(new Queue<Card>(cards), new List<int>(Enumerable.Repeat(1, cards.Count())), 0);
}