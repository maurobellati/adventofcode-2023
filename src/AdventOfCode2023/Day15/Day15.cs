namespace adventofcode2023.Day15;

public static class Day15
{
    public static int Part1(string file) =>
        File.ReadAllLines(file)[0]
            .Split(',')
            .Select(HashMap.Hash)
            .Sum();

    public static int Part2(string file)
    {
        var hashMap = new HashMap();
        foreach (var operation in File.ReadAllLines(file)[0].Split(','))
        {
            if (operation.EndsWith('-'))
            {
                var key = operation.Replace("-", "");
                Console.WriteLine($"remove {key}");
                hashMap.Remove(key);
            }

            if (operation.Contains('='))
            {
                var parts = operation.Split('=');
                var key = parts[0];
                var value = int.Parse(parts[1]);
                Console.WriteLine($"put {key} = {value}");
                hashMap.Put(key, value);
            }
        }

        return hashMap.FocusingPower();
    }

    internal class HashMap
    {
        private readonly List<List<KeyValue>> map = Enumerable.Range(0, 256).Select(_ => new List<KeyValue>()).ToList();

        internal static int Hash(string input) => input.Aggregate(0, (current, c) => (current + c) * 17 % 256);

        public override string ToString() => string.Join("\n", map.Where(v => v.Count > 0).Select(it => string.Join(", ", it.Select(kv => $"{kv.Key} = {kv.Value}"))));

        public int FocusingPower() => map.SelectMany((values, box) => values.Select((lens, i) => (box + 1) * (i + 1) * lens.Value)).ToList().Sum();

        public void Put(string key, int value)
        {
            var values = map[Hash(key)];
            var index = values.FindIndex(kv => kv.Key == key);
            if (index >= 0)
            {
                values[index] = new(key, value);
                return;
            }

            values.Add(new(key, value));
        }

        public void Remove(string key) => map[Hash(key)].RemoveAll(kv => kv.Key == key);

        private sealed record KeyValue(string Key, int Value);
    }
}
