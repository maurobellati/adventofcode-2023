namespace AdventOfCode2023Tests;

using adventofcode2023.Day11;

public class Day11Tests
{
    [Theory]
    [InlineData("Day11/example.txt", 374)]
    [InlineData("Day11/input.txt", 9627977)]
    public void Part1(string file, int expected) => Assert.Equal(expected, Day11.Part1(file));

    [Theory]
    [InlineData("Day11/example.txt", 10, 1030)]
    [InlineData("Day11/example.txt", 100, 8410)]
    [InlineData("Day11/input.txt", 1_000_000, 644248339497)]
    public void Part2(string file, int expansion, long expected) => Assert.Equal(expected, Day11.Part2(file, expansion));
}
