namespace adventofcode2023.Day12;

using System.Diagnostics;
using CSharpFunctionalExtensions;

public static class Day12
{
    private const char Broken = '#';
    private const char Unknown = '?';
    private const char Ok = '.';

    private static readonly Dictionary<CacheKey, long> Cache = new();
    private static int cacheHits;

    public static long Arrangements(string config, List<int> groups) => CountArrangements(config, groups.ToArray());

    public static (string Record, List<int> Groups) Parse(string input, int fold)
    {
        var parts = input.Split(' ');
        var record = string.Join("?", Enumerable.Repeat(parts[0], fold));
        var groups = string.Join(",", Enumerable.Repeat(parts[1], fold));
        return (record, new(groups.Split(',').Select(int.Parse).ToList()));
    }

    public static long Part1(string file) => Solve(file, 1);

    public static long Part2(string file) => Solve(file, 5);

    private static long CountArrangements(string config, int[] groups)
    {
        // add memoization to avoid recomputing the same config/groups
        var key = new CacheKey(config, groups);
        if (Cache.TryGetValue(key, out var cached))
        {
            cacheHits++;
            return cached;
        }

        if (config.Length == 0)
        {
            // valid only if no more groups to consider
            // if groups has any value, this is NOT a valid record
            return groups.Length == 0 ? 1 : 0;
        }

        if (groups.Length == 0)
        {
            // if there are # in the config, this is NOT a valid record
            return config.IndexOf(Broken) == -1 ? 1 : 0;
        }

        var result = 0L;
        var configChar = config[0];

        if (configChar is Broken or Unknown)
        {
            // case when we consider ? as '#', or when we have a '#'
            // we can try to use the first group
            var n = groups[0];
            var enoughSpace = n <= config.Length;
            if (enoughSpace)
            {
                // no functioning spring should be in the next n tiles
                var noFunctioningInThisGroup = config[..n].IndexOf(Ok) == -1;
                // either we are at the end of the config, or the next tile is not a broken spring
                var noBrokenAfterThisGroup = config.Length == n || config[n] != Broken;
                if (noFunctioningInThisGroup && noBrokenAfterThisGroup)
                {
                    var nextConfig = config.Length == n ? string.Empty : config[(n + 1) ..];
                    var nextGroups = groups[1..];
                    result += CountArrangements(nextConfig, nextGroups);
                }
            }
        }

        if (configChar is Ok or Unknown)
        {
            // case when we consider ? as '.', or when we have a '.': we can skip it
            result += CountArrangements(config[1..], groups);
        }

        Cache[key] = result;

        return result;
    }

    private static long Solve(string file, int fold)
    {
        var startTime = Stopwatch.GetTimestamp();
        var lines = File.ReadAllLines(file);
        var arrangements = lines
            .Select(
                (line, i) =>
                {
                    var (config, groups) = Parse(line, fold);
                    Console.Write($"{i + 1}/{lines.Length}) {config} {string.Join(",", groups)}");
                    var start = Stopwatch.GetTimestamp();
                    var arrangements = Arrangements(config, groups);
                    var elapsed = Stopwatch.GetElapsedTime(start);
                    Console.WriteLine($" => {arrangements} arrangements in {elapsed}");
                    return arrangements;
                })
            .ToList();
        var result = arrangements.Sum();
        var elapsed = Stopwatch.GetElapsedTime(startTime);
        Console.WriteLine($"{file} => {result} in {elapsed}");
        Console.WriteLine($"Cache size: {Cache.Count}, {cacheHits} cache hits");
        Console.WriteLine($"Top 10 arrangements: {string.Join(", ", arrangements.OrderByDescending(x => x).Take(10))}");
        return result;
    }

    public class CacheKey : ValueObject
    {
        public CacheKey(string config, int[] groups)
        {
            Config = config;
            Groups = groups;
        }

        public string Config { get; set; }

        public int[] Groups { get; set; }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Config;
            foreach (var group in Groups)
            {
                yield return group;
            }
        }
    }
}
