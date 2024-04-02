namespace adventofcode2023.Day24;

public static class Day24
{
    public static int Part1(string file, long min, long max)
    {
        var values = File.ReadAllLines(file)
            .Select(Parse)
            .ToList();

        // compute for all possible pair of points, if they intersect inside a rectangle [min, max]
        // if they intersect, increment the count

        var result = 0;
        for (var i = 0; i < values.Count; i++)
        {
            for (var j = i + 1; j < values.Count; j++)
            {
                var (p1, d1) = values[i];
                var (p2, d2) = values[j];

                // We have to consider just X and Y, ignore Z

                // line 1: p1 + t1 * d1
                // line 2: p2 + t2 * d2

                // equation of line 1:
                // x = p1.X + t1 * d1.X
                // y = p1.Y + t1 * d1.Y

                // equation of line 2:
                // x = p2.X + t2 * d2.X
                // y = p2.Y + t2 * d2.Y

                // solve for t1 and t2
                // p1.X + t1 * d1.X = p2.X + t2 * d2.X
                // p1.Y + t1 * d1.Y = p2.Y + t2 * d2.Y

                // t1 = (p2.X - p1.X + t2 * d2.X) / d1.X
                // t1 = (p2.Y - p1.Y + t2 * d2.Y) / d1.Y

                // (p2.X - p1.X + t2 * d2.X) / d1.X = (p2.Y - p1.Y + t2 * d2.Y) / d1.Y
                // (p2.X - p1.X) / d1.X + t2 * d2.X / d1.X = (p2.Y - p1.Y) / d1.Y + t2 * d2.Y / d1.Y
                // t2 * d2.X / d1.X - t2 * d2.Y / d1.Y = (p2.Y - p1.Y) / d1.Y - (p2.X - p1.X) / d1.X
                // t2 * (d2.X / d1.X - d2.Y / d1.Y) = (p2.Y - p1.Y) / d1.Y - (p2.X - p1.X) / d1.X
                var t2 = (1.0 * (p2.Y - p1.Y) / d1.Y - 1.0 * (p2.X - p1.X) / d1.X) / (1.0 * d2.X / d1.X - 1.0 * d2.Y / d1.Y);

                var t1 = (p2.X - p1.X + t2 * d2.X) / d1.X;

                if (t1 < 0 || t2 < 0)
                {
                    // the lines are parallel or do not intersect
                    continue;
                }

                // the intersection point is p2 + t2 * d2
                var intersectionX = p2.X + t2 * d2.X;
                var intersectionY = p2.Y + t2 * d2.Y;

                if (intersectionX >= min && intersectionX <= max && intersectionY >= min && intersectionY <= max)
                {
                    result++;
                }
            }
        }

        return result;
    }

    public static int Part2(string file) => 0;

    private static (Point Point, Vector Direction) Parse(string arg)
    {
        // input: 250858894332919, 335837061137784, 250417346929375 @ 175, -88, 23
        // output: (Point(250858894332919, 335837061137784, 250417346929375), Vector(175, -88, 23))
        var parts = arg.Split("@");
        var point = parts[0].Trim().Split(",").Select(long.Parse).ToArray();
        var vector = parts[1].Trim().Split(",").Select(long.Parse).ToArray();
        return (new(point[0], point[1], point[2]), new(vector[0], vector[1], vector[2]));
    }
    // File.ReadAllLines(file).Length;

    public record Vector(long X, long Y, long Z);

    public record Point(long X, long Y, long Z);

    // public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    // public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    // public static Point operator *(Point a, int b) => new(a.X * b, a.Y * b, a.Z * b);
    // public static Point operator /(Point a, int b) => new(a.X / b, a.Y / b, a.Z / b);
}
