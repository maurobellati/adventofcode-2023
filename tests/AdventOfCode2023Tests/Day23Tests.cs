namespace AdventOfCode2023Tests;

using adventofcode2023.Day23;

public class Day23Tests
{
    [Theory]
    [InlineData("Day23/example.txt", 94)]
    [InlineData("Day23/input.txt", 2358)]
    public void Part1(string file, int expected) => Assert.Equal(expected, Day23.Part1(file));

    [Theory]
    [InlineData("Day23/example.txt", 154)]
    [InlineData("Day23/input.txt", 0)]
    public void Part2(string file, int expected) => Assert.Equal(expected, Day23.Part2(file));
}
