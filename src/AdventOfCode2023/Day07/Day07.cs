namespace adventofcode2023.Day07;

public static class Day07
{
    public enum Card
    {
        Joker = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14
    }

    public enum HandType
    {
        HighCard = 0,
        OnePair = 1,
        TwoPairs = 2,
        ThreeOfAKind = 3,
        FullHouse = 4,
        FourOfAKind = 5,
        FiveOfAKind = 6
    }

    public static long Part1(string file) => Solve(file, Part01.CardFromSymbol);

    public static long Part2(string file) => Solve(file, Part02.CardFromSymbol);

    private static HandType InferTypeFromCards(ICollection<Card> cards)
    {
        // count the jokers
        var jokerCount = cards.Count(it => it == Card.Joker);

        // group by card without the jokers
        var groups = cards.Where(it => it != Card.Joker).GroupBy(it => it).Select(it => it.Count()).OrderDescending().ToList();

        // add jokers to the top group
        if (jokerCount != 0)
        {
            if (groups.Count == 0)
            {
                groups.Add(0);
            }

            groups[0] += jokerCount;
        }

        return InferTypeFromGroupedCards(groups);
    }

    private static HandType InferTypeFromGroupedCards(IEnumerable<int> cardGroups) =>
        cardGroups.OrderDescending().ToList() switch
        {
            [5] => HandType.FiveOfAKind,
            [4, 1] => HandType.FourOfAKind,
            [3, 2] => HandType.FullHouse,
            [3, 1, 1] => HandType.ThreeOfAKind,
            [2, 2, 1] => HandType.TwoPairs,
            [2, 1, 1, 1] => HandType.OnePair,
            [1, 1, 1, 1, 1] => HandType.HighCard,

            _ => throw new ArgumentException("Invalid cards", nameof(cardGroups))
        };

    private static long Solve(string file, Func<char, Card> cardFromSymbol) =>
        File.ReadAllLines(file)
            .Select(
                line =>
                {
                    var lineParts = line.Split(' ');
                    var handAsString = lineParts[0];
                    var cards = handAsString.Select(cardFromSymbol).ToList();
                    return (Hand: new Hand(handAsString, cards, InferTypeFromCards(cards)), Bid: long.Parse(lineParts[1]));
                })
            .OrderBy(it => it.Hand)
            .Select(WeightedBid)
            .Sum();

    private static long WeightedBid((Hand Hand, long Bid) handWithBid, int index)
    {
        var rank = index + 1;
        Console.WriteLine("Hand: " + handWithBid.Hand + " Bid: " + handWithBid.Bid + " Rank: " + rank + " WeightedBid: " + handWithBid.Bid * rank);
        return handWithBid.Bid * rank;
    }

    public record Hand : IComparable<Hand>
    {
        private readonly string value;

        public Hand(string value, IEnumerable<Card> cards, HandType type)
        {
            this.value = value;
            Cards = cards.ToList();
            Type = type;
        }

        public List<Card> Cards { get; }

        public HandType Type { get; }

        public int CompareTo(Hand? other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (other is null)
            {
                return 1;
            }

            var typeComparison = Type.CompareTo(other.Type);
            if (typeComparison != 0)
            {
                return typeComparison;
            }

            return Cards.Select((card, i) => card.CompareTo(other.Cards[i]))
                .FirstOrDefault(comparison => comparison != 0);
        }

        public override string ToString() => $"Hand({value}, {Type})";
    }

    private static class Part02
    {
        public static Card CardFromSymbol(char input) =>
            input switch
            {
                'J' => Card.Joker,
                _ => Part01.CardFromSymbol(input)
            };
    }

    private static class Part01
    {
        public static Card CardFromSymbol(char input) =>
            input switch
            {
                '2' => Card.Two,
                '3' => Card.Three,
                '4' => Card.Four,
                '5' => Card.Five,
                '6' => Card.Six,
                '7' => Card.Seven,
                '8' => Card.Eight,
                '9' => Card.Nine,
                'T' => Card.Ten,
                'J' => Card.Jack,
                'Q' => Card.Queen,
                'K' => Card.King,
                'A' => Card.Ace,

                _ => throw new ArgumentOutOfRangeException(nameof(input), input, "Invalid card symbol")
            };
    }
}
