namespace adventofcode2023;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

public static partial class StringExtensions
{
    public static string After(this string input, string value, StringComparison comparisonType = StringComparison.Ordinal)
    {
        var indexOf = input.LastIndexOf(value, comparisonType);
        return indexOf == -1 ? input : input[(indexOf + value.Length)..];
    }

    public static string Before(this string input, string value, StringComparison comparisonType = StringComparison.Ordinal)
    {
        var indexOf = input.IndexOf(value, comparisonType);
        return indexOf == -1 ? input : input[..indexOf];
    }

    public static IEnumerable<T> Extract<T>(this string input, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern, Func<string, T> parser) =>
        Extract(input, pattern).Select(parser);

    public static IEnumerable<string> Extract(this string input, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern) =>
        Regex.Matches(input, pattern).Select(m => m.Value);

    public static IEnumerable<T> Extract<T>(this string input, Regex regex, Func<string, T> parser) =>
        Extract(input, regex).Select(parser);

    public static IEnumerable<string> Extract(this string input, Regex regex) =>
        regex.Matches(input).Select(m => m.Value);

    public static IEnumerable<int> ExtractInts(this string input) => input.Extract(NumberRegex(), int.Parse);

    public static IEnumerable<long> ExtractLongs(this string input) => input.Extract(NumberRegex(), long.Parse);

    public static bool IsBlank(this string input) => string.IsNullOrWhiteSpace(input);

    public static bool IsEmpty(this string input) => string.IsNullOrEmpty(input);

    public static bool IsNotBlank(this string input) => !input.IsBlank();

    public static bool IsNotEmpty(this string input) => !input.IsEmpty();

    public static string[] Lines(this string input) => input.Split(Environment.NewLine);

    public static string[] Lines(this string input, StringSplitOptions options) => input.Split(Environment.NewLine, options);

    public static IEnumerable<(int Row, int Col, char Value)> Scan(this IEnumerable<string> text, Func<char, bool> predicate)
    {
        var row = 0;
        foreach (var line in text)
        {
            var col = 0;
            foreach (var c in line)
            {
                if (predicate(c))
                {
                    yield return (row, col, c);
                }

                col++;
            }

            row++;
        }
    }

    public static string Strip(this string input, string oldValue) => input.Replace(oldValue, string.Empty);

    public static string Strip(this string input, string oldValue, StringComparison comparisonType) => input.Replace(oldValue, string.Empty, comparisonType);

    public static string Strip(this string input, string oldValue, bool ignoreCase, CultureInfo? culture) => input.Replace(oldValue, string.Empty, ignoreCase, culture);

    public static string[] Words(this string input) => input.Words(StringSplitOptions.RemoveEmptyEntries);

    public static string[] Words(this string input, StringSplitOptions options) => input.Split(' ', options);

    [GeneratedRegex("-?\\d+", RegexOptions.Compiled)]
    private static partial Regex NumberRegex();
}
