namespace adventofcode2023;

public static class Extensions
{
    public static bool Between(this int value, int min, int max)
    {
        if (min <= max)
        {
            return min <= value && value <= max;
        }

        return Between(value, max, min);
    }

    public static bool Between(this long value, long min, long max) => min <= value && value <= max;

    public static long Gcd(long a, long b)
    {
        while (b != 0)
        {
            var t = b;
            b = a % b;
            a = t;
        }

        return a;
    }

    public static string Join<T>(this IEnumerable<T> values, string separator) => string.Join(separator, values);

    public static long Lcm(long a, long b)
    {
        var gcd = Gcd(a, b);
        return gcd == 0 ? 0 : a * b / gcd;
    }
}
