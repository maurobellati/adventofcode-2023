namespace adventofcode2023.Day05;

public static class Day05
{
    public record Map(string Label, List<Rule> Rules)
    {
        public long Lookup(long input)
        {
            var rule = Rules.Find(rule => rule.IsMatch(input));
            return rule?.Lookup(input) ?? input;
        }

        public override string ToString() => $"Map({Label}, Rules=[{string.Join(", ", Rules)}])";

        public IEnumerable<Range> Split(Range input)
        {
            List<Range> allUnmapped = [input];
            List<Range> allMapped = [];

            foreach (var rule in Rules)
            {
                var range = allUnmapped.Find(range => rule.SourceRange.Overlaps(range));
                if (range is null)
                {
                    continue;
                }

                allUnmapped.Remove(range);

                var (mapped, unmapped) = rule.Split(range);

                allUnmapped.AddRange(unmapped);

                if (mapped is not null)
                {
                    allMapped.Add(mapped);
                }
            }

            return allMapped.Concat(allUnmapped);
        }
    }

    public class Rule
    {
        public Rule(long sourceStart, long destinationStart, long length)
        {
            Shift = destinationStart - sourceStart;
            Length = length;
            SourceRange = Range.FromStartAndLength(sourceStart, length);
            DestinationRange = Range.FromStartAndLength(destinationStart, length);
        }

        public long Shift { get; }

        public Range DestinationRange { get; }

        public Range SourceRange { get; }

        public bool IsMatch(long input) => SourceRange.Contains(input);

        public long Lookup(long input) => Shift + input;

        public (Range? Mapped, List<Range> Unmapped) Split(Range input)
        {
            if (!SourceRange.Overlaps(input))
            {
                return (null, []);
            }

            var sourceIntersection = input.Intersect(SourceRange);

            List<Range> unmapped = [];
            if (input.End > sourceIntersection.End)
            {
                unmapped.Add(input with { Start = sourceIntersection.End + 1 });
            }

            if (input.Start < sourceIntersection.Start)
            {
                unmapped.Add(input with { End = sourceIntersection.Start - 1 });
            }

            var destinationMapped = sourceIntersection.Shift(Shift);
            return (destinationMapped, unmapped);
        }

        public long Length { get; init; }
    }

    public record Range(long Start, long End)
    {
        public static Range FromStartAndLength(long start, long length) => new(start, start + length - 1);

        public bool Contains(long input) => input.Between(Start, End);

        public Range Intersect(Range other) => new(Math.Max(Start, other.Start), Math.Min(End, other.End));

        public bool Overlaps(Range other) => other.End >= Start && other.Start <= End;

        public Range Shift(long shift) => new(Start + shift, End + shift);

        public override string ToString() => $"[{Start}, {End}]";
    }

    public static long Part1(string file)
    {
        var lines = File.ReadAllLines(file);

        // first line has format: "seeds: 1 2 3 4 5"
        var seeds = lines[0].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(long.Parse).ToList();
        var maps = ParseMaps(lines);
        return seeds.Select(seed => SeedToLocation(maps, seed)).Min();
    }

    public static long Part2(string file)
    {
        var lines = File.ReadAllLines(file);

        // first line has format: "seeds: 1 2 3 4 5 6"
        // seeds must be considered in pairs or Range: (1,2) (3,4) (5,6)
        // chunk seeds into pairs
        var seeds = lines[0].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(long.Parse);
        IEnumerable<Range> ranges = seeds.Chunk(2).Select(x => Range.FromStartAndLength(x[0], x[1])).ToList();
        var maps = ParseMaps(lines);

        var finalRanges = maps.Aggregate(ranges, (stepRanges, map) => stepRanges.SelectMany(map.Split));
        return finalRanges.MinBy(range => range.Start)?.Start ?? 0;
    }

    private static long SeedToLocation(IEnumerable<Map> maps, long seed) => maps.Aggregate(seed, (value, map) => map.Lookup(value));

    private static List<Map> ParseMaps(string[] lines)
    {
        List<Map> result = [];
        // Iterate starting from the 3rd line
        var i = 2;
        while (i < lines.Length)
        {
            // if line has format: "SOURCE-to-DESTINATION map:"
            if (lines[i].EndsWith("map:"))
            {
                var label = lines[i];

                // iterate over the next lines until an empty line is found
                List<Rule> rules = [];
                i++;
                while (i < lines.Length && !string.IsNullOrEmpty(lines[i]))
                {
                    var ruleParts = lines[i].Split(" ").Select(long.Parse).ToArray();
                    rules.Add(new(ruleParts[1], ruleParts[0], ruleParts[2]));
                    i++;
                }

                result.Add(new(label, rules));
                continue;
            }

            i++;
        }

        return result;
    }
}
