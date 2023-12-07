namespace AdventOfCode2023Tests;

using adventofcode2023.Day04;

public class Day04Tests
{
    [Theory]
    [InlineData("Day04/example.txt", 13)]
    [InlineData("Day04/input.txt", 27059)]
    public void Part1(string file, int expected) => Assert.Equal(expected, Day04.Part1(file));

    [Theory]
    [InlineData("Day04/example.txt", 30)]
    [InlineData("Day04/input.txt", 5744979)]
    public void Part2(string file, int expected) => Assert.Equal(expected, Day04.Part2(file));
}
