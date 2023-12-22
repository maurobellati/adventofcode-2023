namespace AdventOfCode2023Tests;

using adventofcode2023.Day16;

public class Day16Tests
{
    [Theory]
    [InlineData("Day16/example.txt", 46)]
    [InlineData("Day16/input.txt", 7939)]
    public void Part1(string file, int expected) => Assert.Equal(expected, Day16.Part1(file));

    [Theory]
    [InlineData("Day16/example.txt", 51)]
    [InlineData("Day16/input.txt", 8318)]
    public void Part2(string file, int expected) => Assert.Equal(expected, Day16.Part2(file));
}
