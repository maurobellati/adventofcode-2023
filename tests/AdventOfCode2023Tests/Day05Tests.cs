namespace AdventOfCode2023Tests;

using adventofcode2023.Day05;
using FluentAssertions;

public class Day05Tests
{
    [Fact]
    public void MapLookupTest()
    {
        var map = new Day05.Map(
            "label",
            [
                new(98, 50, 2),
                new(50, 52, 48)
            ]);

        // Rule 1: 98-99 => 50-51
        Assert.Equal(81, map.Lookup(79));

        // Rule 2: 50-97 => 52-99
        Assert.Equal(52, map.Lookup(50));

        // any number that is NOT mapped by a Rule, is mapped to itself
        Assert.Equal(0, map.Lookup(0));
        Assert.Equal(100, map.Lookup(100));
    }

    [Fact]
    public void MapSplitTest()
    {
        var map = new Day05.Map(
            "label",
            [
                new(98, 50, 2),
                new(50, 52, 48)
            ]);
        var input = new Day05.Range(1, 100);
        var actual = map.Split(input);
        actual.Should().BeEquivalentTo(
        [
            new Day05.Range(1, 49),
            new Day05.Range(50, 51),
            new Day05.Range(52, 99),
            new Day05.Range(100, 100)
        ]);
    }

    [Theory]
    [InlineData("Day05/example.txt", 35)]
    [InlineData("Day05/input.txt", 313045984)]
    public void Part1(string file, int expected) => Assert.Equal(expected, Day05.Part1(file));

    [Theory]
    [InlineData("Day05/example.txt", 46)]
    [InlineData("Day05/input.txt", 20283860)]
    public void Part2(string file, int expected) => Assert.Equal(expected, Day05.Part2(file));

    [Fact]
    public void RuleIsMatchTest()
    {
        var rule = new Day05.Rule(98, 50, 2);

        Assert.True(rule.IsMatch(98));
        Assert.True(rule.IsMatch(99));
        Assert.False(rule.IsMatch(100));
        Assert.False(rule.IsMatch(97));
    }

    [Fact]
    public void RuleLookupTest()
    {
        var rule = new Day05.Rule(98, 50, 2);
        Assert.Equal(50, rule.Lookup(98));
        Assert.Equal(51, rule.Lookup(99));
    }

    [Fact]
    public void RuleSplitTest()
    {
        var rule = new Day05.Rule(98, 50, 2);
        var input = new Day05.Range(1, 100);
        var (mapped, unmapped) = rule.Split(input);

        mapped.Should().Be(new Day05.Range(50, 51));
        unmapped.Should().BeEquivalentTo(new Day05.Range[]{new(1, 97), new(100, 100)});
    }
}
