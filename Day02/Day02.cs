namespace adventofcode2023.Day02;

using System.Text.RegularExpressions;

public static class Day02
{
    public record RgbGame(int Id, List<Rgb> Rgb);

    public record Rgb(int Red, int Green, int Blue);

    public static void Solve(string[] args)
    {
        var input = File.ReadAllLines($"Day02/{args.FirstOrDefault() ?? "input.txt"}");

        Console.WriteLine($"Part 1: {Part1(input)}");
        Console.WriteLine($"Part 2: {Part2(input)}");
    }

    private static int Part1(IEnumerable<string> input) =>
        //12 red, 13 green, and 14 blu
        input.Select(ParseRgbGame)
            .Where(game => game.Rgb.TrueForAll(rgb => rgb is { Red: <= 12, Green: <= 13, Blue: <= 14 }))
            .Select(game => game.Id)
            .Sum();

    private static RgbGame ParseRgbGame(string line)
    {
        // extract Id from: "Game {ID}: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green"
        var id = int.Parse(Regex.Match(line, @"Game (\d+):").Groups[1].Value);
        // get substring after :
        var rgbGames = line.Substring(line.IndexOf(':') + 1);
        var rgbs = rgbGames.Split(";").Select(ParseRgb).ToList();
        return new(id, rgbs);
    }

    private static Rgb ParseRgb(string input) =>
        // input format: 1 red, 2 green, 6 blue
        // red, green, blue: are optional and can be in any order, but just once per input
        new(GetComponent(input, "red"), GetComponent(input, "green"), GetComponent(input, "blue"));

    private static int GetComponent(string input, string component)
    {
        var value = Regex.Match(input, @"(\d+) " + component).Groups[1].Value;
        return string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    private static int Part2(IEnumerable<string> input) =>
        input.Select(ParseRgbGame)
            .Select(game => MinRgb(game.Rgb))
            .Select(rgb => rgb.Red * rgb.Green * rgb.Blue)
            .Sum();

    private static Rgb MinRgb(IReadOnlyCollection<Rgb> rgbs)
    {
        // for each component, get the max value
        var red = rgbs.Max(rgb => rgb.Red);
        var green = rgbs.Max(rgb => rgb.Green);
        var blue = rgbs.Max(rgb => rgb.Blue);
        return new(red, green, blue);
    }
}
