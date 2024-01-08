namespace AdventOfCode2023Tests;

using adventofcode2023.Day19;

public class Day19Tests
{
    [Theory]
    [InlineData("Day19/example.txt", 19114)]
    [InlineData("Day19/input.txt", 509597)]
    public void Part1(string file, int expected) => Assert.Equal(expected, Day19.Part1(file));

    [Theory]
    [InlineData("Day19/example.txt", 167409079868000)]
    [InlineData("Day19/input.txt", 143219569011526)]
    public void Part2(string file, long expected) => Assert.Equal(expected, Day19.Part2(file));
}
