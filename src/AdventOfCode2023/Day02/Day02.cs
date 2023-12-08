namespace adventofcode2023.Day02;

using System.Text.RegularExpressions;

public static class Day02
{
    public static int Part1(string file) =>
        //12 red, 13 green, and 14 blu
        File.ReadAllLines(file)
            .Select(ParseGame)
            .Where(game => game.Matches.TrueForAll(rgb => rgb is { Red: <= 12, Green: <= 13, Blue: <= 14 }))
            .Select(game => game.Id)
            .Sum();

    public static int Part2(string file) =>
        File.ReadAllLines(file)
            .Select(ParseGame)
            .Select(game => MaxMatch(game.Matches))
            .Select(match => match.Red * match.Green * match.Blue)
            .Sum();

    private static int GetComponent(string input, string component)
    {
        var value = Regex.Match(input, $@"(\d+) {component}").Groups[1].Value;
        return string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    private static Match MaxMatch(IReadOnlyCollection<Match> rgbs) =>
        // for each component, get the max value
        new(
            rgbs.Max(rgb => rgb.Red),
            rgbs.Max(rgb => rgb.Green),
            rgbs.Max(rgb => rgb.Blue));

    private static Game ParseGame(string line)
    {
        // extract Id from: "Game {ID}: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green"
        var id = int.Parse(Regex.Match(line, @"Game (\d+):").Groups[1].Value);
        // get substring after :
        var matchesSubstring = line[(line.IndexOf(':') + 1)..];
        var matches = matchesSubstring.Split(";").Select(ParseMatch).ToList();
        return new(id, matches);
    }

    private static Match ParseMatch(string input) =>
        // input format: 1 red, 2 green, 6 blue
        // red, green, blue: are optional and can be in any order, but just once per input
        new(GetComponent(input, "red"), GetComponent(input, "green"), GetComponent(input, "blue"));

    public record Game(int Id, List<Match> Matches);

    public record Match(int Red, int Green, int Blue);
}
