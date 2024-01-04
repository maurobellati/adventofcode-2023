namespace AdventOfCode2023Tests;

using adventofcode2023.Day21;

public class Day21Tests
{
    [Theory]
    [InlineData("Day21/example.txt", 6, 16)]
    [InlineData("Day21/input.txt", 64, 3770)]
    public void Part1(string file, int steps, int expected) => Assert.Equal(expected, Day21.Part1(file, steps));

    [Theory]
    [InlineData("Day21/input.txt", 26501365, 628206330073385)]
    public void Part2(string file, int steps, long expected) => Assert.Equal(expected, Day21.Part2Quadratic(file, steps));

    [Theory]
    [InlineData("Day21/example.txt", 6, 16)]
    [InlineData("Day21/example.txt", 10, 50)]
    [InlineData("Day21/example.txt", 50, 1594)]
    [InlineData("Day21/example.txt", 100, 6536)]
    [InlineData("Day21/example.txt", 500, 167004)]
    [InlineData("Day21/example.txt", 1000, 668697)]
    public void Part2Example(string file, int steps, long expected) => Assert.Equal(expected, Day21.Part2SimpleBfs(file, steps));
}
