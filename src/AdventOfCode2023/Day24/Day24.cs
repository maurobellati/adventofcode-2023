namespace adventofcode2023.Day24;

public static class Day24
{
    public static int Part1(string file, long min, long max)
    {
        var rays = File.ReadAllLines(file)
            .Select(Parse)
            .ToList();

        // compute for all possible pair of points, if they intersect inside a rectangle [min, max]
        // if they intersect, increment the count

        var result = 0;
        for (var i = 0; i < rays.Count; i++)
        {
            for (var j = i + 1; j < rays.Count; j++)
            {
                if (DoRaysIntersectInvertingMatrix(rays[i], rays[j], min, max))
                {
                    result++;
                }
            }
        }

        return result;
    }

    public static int Part2(string file) => 0;

    private static bool DoRaysIntersect(Ray r1, Ray r2, long min, long max)
    {
        var (p1, v1) = r1;
        var (p2, v2) = r2;
        // We have to consider just X and Y, ignore Z

        // equation of line 1:
        // x = p1.X + t1 * v1.X
        // y = p1.Y + t1 * v1.Y

        // equation of line 2:
        // x = p2.X + t2 * v2.X
        // y = p2.Y + t2 * v2.Y

        // solve for t1 and t2
        // p1.X + t1 * v1.X = p2.X + t2 * v2.X
        // p1.Y + t1 * v1.Y = p2.Y + t2 * v2.Y

        // t1 = (p2.X - p1.X + t2 * v2.X) / v1.X
        // t1 = (p2.Y - p1.Y + t2 * v2.Y) / v1.Y

        // (p2.X - p1.X + t2 * v2.X) / v1.X = (p2.Y - p1.Y + t2 * v2.Y) / v1.Y
        // (p2.X - p1.X) / v1.X + t2 * v2.X / v1.X = (p2.Y - p1.Y) / v1.Y + t2 * v2.Y / v1.Y
        // t2 * v2.X / v1.X - t2 * v2.Y / v1.Y = (p2.Y - p1.Y) / v1.Y - (p2.X - p1.X) / v1.X
        // t2 * (v2.X / v1.X - v2.Y / v1.Y) = (p2.Y - p1.Y) / v1.Y - (p2.X - p1.X) / v1.X
        double dX = p2.X - p1.X;
        double dY = p2.Y - p1.Y;

        var t2 = (dY / v1.Y - dX / v1.X) / (1.0 * v2.X / v1.X - 1.0 * v2.Y / v1.Y);

        var t1 = (dX + t2 * v2.X) / v1.X;

        if (t1 < 0 || t2 < 0)
        {
            // the lines are parallel or do not intersect
            return false;
        }

        // the intersection point is p2 + t2 * d2
        var intersectionX = p2.X + t2 * v2.X;
        var intersectionY = p2.Y + t2 * v2.Y;
        return intersectionX >= min && intersectionX <= max && intersectionY >= min && intersectionY <= max;
    }

    private static bool DoRaysIntersectInvertingMatrix(Ray r1, Ray r2, long min, long max)
    {
        var (p1, v1) = r1;
        var (p2, v2) = r2;
        // We have to consider just X and Y, ignore Z

        // equation of line 1:
        // x = p1.X + t1 * v1.X
        // y = p1.Y + t1 * v1.Y

        // equation of line 2:
        // x = p2.X + t2 * v2.X
        // y = p2.Y + t2 * v2.Y

        // equation system:
        // p1.X + t1 * v1.X = p2.X + t2 * v2.X
        // p1.Y + t1 * v1.Y = p2.Y + t2 * v2.Y

        // t1 * v1.X - t2 * v2.X = p2.X - p1.X
        // t1 * v1.Y - t2 * v2.Y = p2.Y - p1.Y

        // extract matrix: V * T = P
        // | v1.X - v2.X | | t1 | = | p2.X - p1.X |
        // | v1.Y - v2.Y | | t2 | = | p2.Y - p1.Y |

        // solve for T
        // T = V^-1 * P
        // | t1 | = | v1.X -v2.X |^-1 | p2.X - p1.X |
        // | t2 | = | v1.Y -v2.Y |    | p2.Y - p1.Y |

        // V^-1 = 1 / det(V) * | -v2.Y  v2.X |
        //                     | -v1.Y  v1.X |

        var a = v1.X;
        var b = -v2.X;
        var c = v1.Y;
        var d = -v2.Y;

        var detV = a * d - b * c;

        if (detV == 0)
        {
            // the lines are parallel or do not intersect
            return false;
        }

        var dX = p2.X - p1.X;
        var dY = p2.Y - p1.Y;

        // T = V^-1 * P
        var t1 = 1.0 * (d * dX - b * dY) / detV;
        var t2 = 1.0 * (-c * dX + a * dY) / detV;

        if (t1 < 0 || t2 < 0)
        {
            // the lines are parallel or do not intersect
            return false;
        }

        // the intersection point is p2 + t2 * v2
        // x = p2.X + t2 * v2.X
        // y = p2.Y + t2 * v2.Y
        var intersectionX = p2.X + t2 * v2.X;
        var intersectionY = p2.Y + t2 * v2.Y;
        return intersectionX >= min && intersectionX <= max && intersectionY >= min && intersectionY <= max;
    }

    private static Ray Parse(string input)
    {
        // input: 250858894332919, 335837061137784, 250417346929375 @ 175, -88, 23
        var point = input.Before("@").ExtractLongs().ToList();
        var vector = input.After("@").ExtractLongs().ToList();
        return new(new(point[0], point[1], point[2]), new(vector[0], vector[1], vector[2]));
    }

    private sealed record Ray(Point Point, Vector Velocity);

    private sealed record Vector(long X, long Y, long Z);

    private sealed record Point(long X, long Y, long Z);
}
