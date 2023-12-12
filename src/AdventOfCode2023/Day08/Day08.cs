namespace adventofcode2023.Day08;

using LR = (string Left, string Right);

public static class Day08
{
    public static long Part1(string file)
    {
        // line 0: a string of "LR" characters, each representing a left or right turn
        var lines = File.ReadAllLines(file).ToList();
        var instructions = lines[0];

        var network = ParseNetwork(lines);

        var start = "AAA";
        return CountStep(start, node => node == "ZZZ", instructions, network);
    }

    public static long Part2(string file)
    {
        // line 0: a string of "LR" characters, each representing a left or right turn
        var lines = File.ReadAllLines(file).ToList();
        var instructions = lines[0];

        var network = ParseNetwork(lines);

        var startNodes = network.Keys.Where(it => it.EndsWith('A')).ToList();

        var steps = startNodes.Select(node => CountStep(node, node => node.EndsWith('Z'), instructions, network)).ToList();
        Console.WriteLine($"steps: {string.Join(", ", steps)}");

        // return least common multiple of all steps
        return steps.Aggregate(Extensions.Lcm);
    }

    private static long CountStep(string start, Predicate<string> goal, string instructions, Dictionary<string, LR> network)
    {
        var step = 0;
        var node = start;
        while (!goal(node))
        {
            var instruction = instructions[step++ % instructions.Length];
            var (left, right) = network[node];
            node = instruction == 'L' ? left : right;
            // Console.WriteLine("Step {0}: {1} => {2}", step, instruction, node);
        }

        return step;
    }

    private static Dictionary<string, LR> ParseNetwork(List<string> lines) =>
        // line 2 to n: labeled nodes as: "AAA = (BBB, CCC)"
        lines[2..].ToDictionary(line => line[..3], line => (line[7..10], line[12..15]));
}
