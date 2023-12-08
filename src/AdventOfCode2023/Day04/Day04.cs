namespace adventofcode2023.Day04;

using System.Text.RegularExpressions;

public static class Day04
{
    public static int Part1(string file) =>
        File.ReadAllLines(file)
            .Select(ParseCard)
            .Select(Points)
            .Sum();

    public static int Part2(string file)
    {
        var originalCards = File.ReadAllLines(file)
            .Select(ParseCard)
            .ToList();

        // Initialize an array of copies, 1 for each card
        var copies = Enumerable.Repeat(1, originalCards.Count).ToArray();

        // iterate over all cards
        for (var i = 0; i < originalCards.Count; i++)
        {
            var card = originalCards[i];
            var currentCardCopies = copies[i];
            var matchingCount = card.MatchingNumbers.Count;
            Console.WriteLine($"Processing {card} with count={currentCardCopies} and {matchingCount} matching numbers");

            if (matchingCount == 0)
            {
                // if the card has no matching numbers, nothing to do
                Console.WriteLine("  No matching numbers");
                continue;
            }

            // increment the next n-matchingCount card by the number of copies of the current card
            Enumerable.Range(i + 1, matchingCount)
                .ToList()
                .ForEach(
                    j =>
                    {
                        // increment the number of copies of the card with index j by the number of copies of the current card
                        copies[j] += currentCardCopies;
                        Console.WriteLine($"  Incremented copies of card {j + 1} by {currentCardCopies} => {copies[j]}");
                    });
        }

        // the result is the total number of copies
        var result = copies.Sum();
        Console.WriteLine($"Counts: [{string.Join(", ", copies)}] => sum = {result}");
        return result;
    }

    private static Card ParseCard(string input, int i)
    {
        // input is in the format: "Card N: WINNING-NUMBERS | NUMBERS"
        // example: "Card 1: 1 2 3 4 5 | 1 2 3 4 5 6 7 8 9 10"
        var match = Regex.Match(input, @"Card\s+(?<Id>\d+): (?<winningNumbers>[\d ]*)\|(?<numbers>[\d ]*)");
        if (!match.Success)
        {
            throw new ArgumentException("Invalid format", nameof(input));
        }

        return new(
            int.Parse(match.Groups["Id"].Value),
            ParseInts(match.Groups["numbers"].Value).ToArray(),
            ParseInts(match.Groups["winningNumbers"].Value).ToArray());
    }

    private static IEnumerable<int> ParseInts(string value) => value.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);

    private static int Points(Card card)
    {
        // The points is the power of 2 of the number of winning numbers that are in the list of numbers - 1
        var result = (int)Math.Pow(2, card.MatchingNumbers.Count - 1);
        Console.WriteLine($"{card} => count={card.MatchingNumbers.Count} => 2^{card.MatchingNumbers.Count - 1} = {result}");
        return result;
    }

    private sealed class Card
    {
        public Card(int id, ICollection<int> numbers, ICollection<int> winningNumbers)
        {
            Id = id;
            Numbers = numbers.ToHashSet();
            WinningNumbers = winningNumbers.ToHashSet();
            MatchingNumbers = numbers.Intersect(winningNumbers).ToHashSet();
        }

        public int Id { get; }

        public HashSet<int> MatchingNumbers { get; }

        public HashSet<int> Numbers { get; }

        public HashSet<int> WinningNumbers { get; }

        public override string ToString() => $"Card(Id={Id}, All=[{string.Join(", ", Numbers)}], Winning=[{string.Join(", ", WinningNumbers)}], Matching=[{string.Join(", ", MatchingNumbers)}])";
    }
}
