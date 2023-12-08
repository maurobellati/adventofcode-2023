namespace AdventOfCode2023Tests;

using adventofcode2023.Day07;

public class Day07Tests
{
    [Theory]
    [InlineData("Day07/example.txt", 6440)]
    [InlineData("Day07/input.txt", 251287184)]
    public void Part1(string file, int expected) => Assert.Equal(expected, Day07.Part1(file));

    [Theory]
    [InlineData("Day07/example.txt", 5905)]
    [InlineData("Day07/input.txt", 250757288)]
    public void Part2(string file, int expected) => Assert.Equal(expected, Day07.Part2(file));
}
