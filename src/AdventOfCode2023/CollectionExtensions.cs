namespace adventofcode2023;

public static class CollectionExtensions
{
    public static bool IsEmpty<T>(this IEnumerable<T> input) => !input.Any();

    public static bool IsNotEmpty<T>(this IEnumerable<T> input) => input.Any();

    public static string Join<T>(this IEnumerable<T> values, string? separator = null) => string.Join(separator, values);

    public static void Pop<T>(this Stack<T> input, int count)
    {
        for (var i = 0; i < count; i++)
        {
            input.Pop();
        }
    }

    public static void PushRange<T>(this Stack<T> input, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            input.Push(item);
        }
    }
}
