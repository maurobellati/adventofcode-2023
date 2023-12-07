namespace adventofcode2023;

public static class Extensions
{
    public static bool Between(this int value, int min, int max) => min <= value && value <= max;
}
