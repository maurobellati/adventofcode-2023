namespace adventofcode2023.Day19;

using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Part = Dictionary<string, int>;
using Workflows = Dictionary<string, Day19.Workflow>;

public static partial class Day19
{
    private static readonly List<string> RatingFields = ["x", "m", "a", "s"];

    public static long Part1(string file)
    {
        var workflows = ParseWorkflows(file);
        var parts = ParseParts(file);

        return parts
            .Select(part => EvaluatePart(workflows, "in", part))
            .Sum();
    }

    public static long Part2(string file)
    {
        var workflows = ParseWorkflows(file);
        return CountCombinations(workflows, "in", []);
    }

    private static long CalculateCombinations(IReadOnlyCollection<Rule> rules) =>
        // for each field, find the most restrictive range
        RatingFields
            .Select(
                field =>
                {
                    var range = CollectRange(rules, field);
                    Console.WriteLine($"  {field} => {range}");
                    return range;
                })
            .Select(range => range.Length)
            // multiply the lengths of the ranges to calculate the number of combinations
            .Aggregate(1L, (a, b) => a * b);

    private static Range CollectRange(IReadOnlyCollection<Rule> rules, string field) =>
        // intersect the rule belonging to the field. If there is no rule, return a full range.
        rules.Where(rule => rule.Field == field)
            .Select(rule => rule.Range).Aggregate(Range.Full, Range.Intersect);

    private static long CountCombinations(Workflows workflows, string currentWorkflowName, Stack<Rule> evaluatedRules)
    {
        switch (currentWorkflowName)
        {
            case Workflow.RejectedName:
                return 0L;
            case Workflow.AcceptedName:
            {
                Console.WriteLine($"Accepted. Calculating combinations for [{evaluatedRules.Join(", ")}]:");
                var combinations = CalculateCombinations(evaluatedRules);
                Console.WriteLine($"  => {combinations}");
                Console.WriteLine();
                return combinations;
            }
        }

        var result = 0L;

        // negative rules are the rules that were evaluated before the current rule
        // ex: if the first rule is "a<2006:b", when we evaluate the second one, we should negate the first and consider it as "a>=2006"
        // and so on for the following rules
        List<Rule> negativeRules = [];
        var currentWorkflow = workflows[currentWorkflowName];
        foreach (var rule in currentWorkflow.Rules)
        {
            var nextWorkflowName = rule.NextWorkflowName;

            evaluatedRules.Push(rule);
            evaluatedRules.PushRange(negativeRules);

            result += CountCombinations(workflows, nextWorkflowName, evaluatedRules);

            evaluatedRules.Pop(negativeRules.Count);
            evaluatedRules.Pop();

            negativeRules.Add(rule.Negate());
        }

        evaluatedRules.PushRange(negativeRules);
        result += CountCombinations(workflows, currentWorkflow.DefaultWorkflowName, evaluatedRules);
        evaluatedRules.Pop(negativeRules.Count);

        return result;
    }

    private static long EvaluatePart(Workflows workflows, string currentWorkflowName, Part part) =>
        currentWorkflowName switch
        {
            // base cases
            Workflow.RejectedName => 0L,
            Workflow.AcceptedName => part.Values.Sum(),

            // recursive case
            _ => EvaluatePart(workflows, workflows[currentWorkflowName].Dispatch(part), part)
        };

    private static Part ParsePart(string line) =>
        // example: "{x=787,m=2655,a=1222,s=2876}"
        new(
            PartRegex()
                .Matches(line)
                .Select(m => new KeyValuePair<string, int>(m.Groups["field"].Value, int.Parse(m.Groups["value"].Value))));

    private static List<Part> ParseParts(string file) =>
        File.ReadLines(file).SkipWhile(line => !string.IsNullOrWhiteSpace(line)).Skip(1).Select(ParsePart).ToList();

    private static Rule ParseRule(string input)
    {
        // example: "a<2006:b", or "m>2090:c"
        var match = RuleRegex().Match(input);

        if (!match.Success)
        {
            throw new InvalidOperationException($"Unable to parse rule {input}");
        }

        var op = match.Groups["op"].Value[0];
        var value = int.Parse(match.Groups["value"].Value);

        return new(
            match.Groups["field"].Value,
            op switch
            {
                '>' => Range.GreaterThan(value),
                '<' => Range.LessThan(value),
                _ => throw new NotImplementedException()
            },
            match.Groups["nextWorkflow"].Value);
    }

    private static Workflow ParseWorkflow(string line)
    {
        // example: "px{a<2006:qkq,m>2090:A,rfg}"

        // name is the first part before the first '{'
        var name = line[..line.IndexOf('{')];

        var body = line[name.Length..^1].Split(',');

        // parse the rule except the last one
        var rules = body.SkipLast(1).Select(ParseRule).ToList();

        // last one is the default workflow
        var defaultWorkflowName = body.Last();

        return new(name, rules, defaultWorkflowName);
    }

    private static Workflows ParseWorkflows(string file) =>
        File.ReadLines(file).TakeWhile(line => !string.IsNullOrWhiteSpace(line)).Select(ParseWorkflow).ToDictionary(w => w.Name, w => w);

    [GeneratedRegex(@"(?<field>\w)=(?<value>\d+)")]
    private static partial Regex PartRegex();

    [GeneratedRegex(@"(?<field>\w)(?<op>[<>])(?<value>\d+):(?<nextWorkflow>\w+)?")]
    private static partial Regex RuleRegex();

    internal record Workflow(string Name, List<Rule> Rules, string DefaultWorkflowName)
    {
        public const string AcceptedName = "A";

        public const string RejectedName = "R";

        public string Dispatch(Part part) =>
            // find first rule that matches and select it's next workflow, otherwise return the default workflow
            Rules.TryFirst(rule => rule.Range.Includes(part[rule.Field])).Map(it => it.NextWorkflowName)
                .GetValueOrDefault(DefaultWorkflowName);
    }

    internal record Rule(string Field, Range Range, string NextWorkflowName)
    {
        public override string ToString() => $"Rule(if {Field} in [{Range.Start}..{Range.End}] => {NextWorkflowName})";

        public Rule Negate() => this with { Range = Range.Negate() };
    }

    internal record Range(int Start, int End)
    {
        private const int MinValue = 1;
        private const int MaxValue = 4000;

        public static Range Full => new(MinValue, MaxValue);

        public int Length => End - Start + 1;

        public static Range GreaterThan(int value) => new(value + 1, MaxValue);

        public static Range Intersect(Range a, Range b) => new(Math.Max(a.Start, b.Start), Math.Min(a.End, b.End));

        public static Range LessThan(int value) => new(MinValue, value - 1);

        public override string ToString() => $"Range([{Start}..{End}], Length={Length})";

        public bool Includes(int value) => value >= Start && value <= End;

        public Range Negate()
        {
            if (MinValue == Start)
            {
                return new(End + 1, MaxValue);
            }

            if (MaxValue == End)
            {
                return new(MinValue, Start - 1);
            }

            throw new InvalidOperationException($"Unable to negate range {this}");
        }
    }
}
