namespace AdventOfCode2023Tests;

using adventofcode2023.Day03;

public class Day03Tests
{
    [Theory]
    [InlineData("Day03/example.txt", 4361)]
    [InlineData("Day03/input.txt", 530495)]
    public void Day03Part1(string file, int expected) => Assert.Equal(expected, Day03.Part1(file));

    [Theory]
    [InlineData("Day03/example.txt", 467835)]
    [InlineData("Day03/input.txt", 80253814)]
    public void Day03Part2(string file, int expected) => Assert.Equal(expected, Day03.Part2(file));
}
