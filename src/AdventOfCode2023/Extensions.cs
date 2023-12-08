namespace adventofcode2023;

public static class Extensions
{
    public static bool Between(this int value, int min, int max) => min <= value && value <= max;
    public static bool Between(this long value, long min, long max) => min <= value && value <= max;

    public static string Join<T>(this IEnumerable<T> values, string separator) => string.Join(separator, values);

}
