namespace adventofcode2023.Day08;

using LR = (string Left, string Right);

public static class Day08
{
    public static long Part1(string file)
    {
        var instructions = ParseInstructions(file);
        var network = ParseNetwork(file);

        var start = "AAA";
        return CountStep(start, node => node == "ZZZ", instructions, network);
    }

    public static long Part2(string file)
    {
        var instructions = ParseInstructions(file);
        var network = ParseNetwork(file);

        var startNodes = network.Keys.Where(it => it.EndsWith('A')).ToList();
        var steps = startNodes.Select(start => CountStep(start, node => node.EndsWith('Z'), instructions, network)).ToList();
        Console.WriteLine($"steps: {string.Join(", ", steps)}");

        // return least common multiple of all steps
        return steps.Aggregate(MathExtensions.Lcm);
    }

    private static long CountStep(string start, Predicate<string> goal, string instructions, Dictionary<string, LR> network)
    {
        var step = 0;
        var node = start;
        while (!goal(node))
        {
            var instruction = instructions[step % instructions.Length];
            step++;
            node = instruction == 'L' ? network[node].Left : network[node].Right;
            // Console.WriteLine("Step {0}: {1} => {2}", step, instruction, node);
        }

        return step;
    }

    private static string ParseInstructions(string file) =>
        // line 0: a string of "LR" characters, each representing a left or right turn
        File.ReadLines(file).First();

    private static Dictionary<string, LR> ParseNetwork(string file) =>
        // line 2 to n: labeled nodes as: "AAA = (BBB, CCC)"
        File.ReadLines(file).Skip(2).ToDictionary(line => line[..3], line => (line[7..10], line[12..15]));
}
