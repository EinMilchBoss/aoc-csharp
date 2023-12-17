using System.Collections.Immutable;

namespace Day04;

public static class IEnumerableExtensions
{
    public static int TotalAmountOfCards(this IEnumerable<Card> cards)
    {
        static int Iterate(ImmutableQueue<Card> cards, ImmutableQueue<int> cardAmounts, int score)
        {
            if (!cards.Any())
                return score;

            var card = cards.Peek();
            var cardAmount = cardAmounts.Peek();

            return Iterate
            (
                cards.Dequeue(),
                UpdateCardAmounts(cardAmounts.Dequeue(), card.GetMatches(), cardAmount),
                score + cardAmount
            );
        }

        return Iterate
        (
            ImmutableQueue.CreateRange(cards),
            ImmutableQueue.CreateRange(Enumerable.Repeat(1, cards.Count())),
            0
        );
    }

    private static ImmutableQueue<int> UpdateCardAmounts(ImmutableQueue<int> cardAmounts, int matches, int increase)
    {
        if (matches == 0)
            return cardAmounts;

        var newCardAmount = cardAmounts.Peek() + increase;
        var newCardAmounts = UpdateCardAmounts(cardAmounts.Dequeue(), matches - 1, increase)
            .Prepend(newCardAmount);
        return ImmutableQueue.CreateRange(newCardAmounts);
    }
}