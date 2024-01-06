namespace AdventOfCode2023Tests;

using adventofcode2023.Day20;

public class Day20Tests
{
    [Theory]
    [InlineData("Day20/example.txt", 32000000)]
    [InlineData("Day20/example-2.txt", 11687500)]
    [InlineData("Day20/input.txt", 898731036)]
    public void Part1(string file, int expected) => Assert.Equal(expected, Day20.Part1(file));

    [Theory]
    [InlineData("Day20/input.txt", 229414480926893)]
    public void Part2(string file, long expected) => Assert.Equal(expected, Day20.Part2(file));
}
